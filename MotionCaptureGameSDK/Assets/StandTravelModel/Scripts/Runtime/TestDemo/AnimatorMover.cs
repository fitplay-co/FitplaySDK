using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorMover : MonoBehaviour
    {
        private const int footIndexLeft = -1;
        private const int footIndexRight = 1;
        private const float footHeightDetach = 0.02f;

        [SerializeField] private int curFootIndex;
        [SerializeField] private Vector3 footPos;
        [SerializeField] private Vector3 deltaPos;
        [SerializeField] private Vector3 anchorPos;
        [SerializeField] private Transform footLeft;
        [SerializeField] private Transform footRight;

        private void Awake() {
            var animator = GetComponent<Animator>();
            footLeft = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            footRight = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        private void LateUpdate() {
            UpdateTouchingFoot();
            UpdatePosWithAnchor();
        }

        /// <summary>
        /// Callback for processing animation movements for modifying root motion.
        /// </summary>
        void OnAnimatorMove()
        {
            //override the root motion
        }

        private void UpdatePosWithAnchor()
        {
            if(curFootIndex != 0)
            {
                footPos = GetFootPos(curFootIndex);
                deltaPos = footPos - anchorPos;
                transform.position -= deltaPos;
            }
        }

        private void UpdateTouchingFoot()
        {
            var touchingFoot = GetTouchingFoot();
            if(touchingFoot != curFootIndex)
            {
                curFootIndex = touchingFoot;
                anchorPos = GetAnchorPos(curFootIndex);
            }
        }

        private int GetTouchingFoot()
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

        private Transform GetAnchor(int foot)
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
            return GetAnchor(foot).position;
        }

        private Vector3 GetFootPos(int foot)
        {
            return GetAnchor(foot).position;
        }
    }
}
