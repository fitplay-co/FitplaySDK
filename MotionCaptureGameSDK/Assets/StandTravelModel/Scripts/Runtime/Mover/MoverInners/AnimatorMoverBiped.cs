using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public class AnimatorMoverBiped : IAnimatorMoverBiped
    {
        private const float footHeightDetach = 0.02f;

        protected const int footIndexLeft = -1;
        protected const int footIndexRight = 1;

        private int curFootIndex;
        private Vector3 anchorPos;
        private Transform footLeft;
        private Transform footRight;
        private Transform transform;
        private IAnimatorMoverReactor moverReactor;

        public AnimatorMoverBiped(Transform transform)
        {
            var animator = transform.GetComponent<Animator>();
            this.footLeft = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            this.footRight = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            this.transform = transform;
            this.moverReactor = transform.GetComponent<IAnimatorMoverReactor>();
        }

        public void OnUpdate(AnimatorStateInfo stateInfo)
        {
            UpdateTouchingFoot(stateInfo);
            UpdatePosWithAnchor();
        }

        protected virtual int GetTouchingFoot(AnimatorStateInfo stateInfo)
        {
            var heightGap = footLeft.position.y - footRight.position.y;
            if(heightGap < -footHeightDetach)
            {
                return footIndexLeft;
            }

            if(heightGap > footHeightDetach)
            {
                return footIndexRight;
            }

            return 0;
        }

        protected virtual Vector3 GetMoveDelta()
        {
            var deltaPos = GetFootPos(curFootIndex) - anchorPos;
            deltaPos.y = 0;
            anchorPos = GetAnchorPos(curFootIndex);
            return deltaPos;
        }

        protected virtual void UpdatePosWithAnchor()
        {
            if(curFootIndex != 0)
            {
                var deltaPos = GetMoveDelta();
                moverReactor.SetAnimatorDelta(-transform.forward * deltaPos.z);
            }
        }

        private void UpdateTouchingFoot(AnimatorStateInfo stateInfo)
        {
            var touchingFoot = GetTouchingFoot(stateInfo);
            if(touchingFoot != curFootIndex)
            {
                curFootIndex = touchingFoot;
                anchorPos = GetAnchorPos(curFootIndex);
            }
        }

        protected Transform GetAnchor(int foot)
        {
            if(foot == footIndexRight)
            {
                return footRight;
            }

            if(foot == footIndexLeft)
            {
                return footLeft;
            }

            return transform;
        }

        private Vector3 GetAnchorPos(int foot)
        {
            return transform.InverseTransformPoint(GetAnchor(foot).position);
        }

        protected Vector3 GetFootPos(int foot)
        {
            return transform.InverseTransformPoint(GetAnchor(foot).position);
        }

        public void OnStart()
        {
            moverReactor.OnAnimatorMoveStart();
        }

        protected float GetRunSpeedScale()
        {
            return moverReactor.GetRunSpeedScale();
        }
    }
}