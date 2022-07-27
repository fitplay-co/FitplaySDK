using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public class AnimatorMoverBipedStepProgress : AnimatorMoverBiped
    {
        private float leftFootStart;
        private float leftFootEnd;
        private int animIdStepProgress;
        private Animator animator;

        public AnimatorMoverBipedStepProgress(Transform transform, float leftFootStart, float leftFootEnd) : base(transform)
        {
            this.leftFootStart = leftFootStart;
            this.leftFootEnd = leftFootEnd;
            this.animator = transform.GetComponent<Animator>();
            this.animIdStepProgress = Animator.StringToHash("stepProgress");
        }

        protected override int GetTouchingFoot(AnimatorStateInfo stateInfo)
        {
            var isLeft = false;
            var stepProgress = animator.GetFloat(animIdStepProgress);
            stepProgress -= (int)stepProgress;

            if(leftFootStart < leftFootEnd)
            {
                isLeft = stepProgress <= leftFootEnd && stepProgress >= leftFootStart;
            }
            else
            {
                isLeft = stepProgress >= leftFootEnd && stepProgress <= leftFootStart;
            }

            return isLeft ? footIndexLeft : footIndexRight;
        }
    }
}