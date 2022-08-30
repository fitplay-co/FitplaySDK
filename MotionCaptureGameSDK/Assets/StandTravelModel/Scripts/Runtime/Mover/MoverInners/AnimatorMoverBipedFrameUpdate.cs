using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public class AnimatorMoverBipedStepProgress : AnimatorMoverBiped
    {
        private float speedScale;
        private float leftFootStart;
        private float leftFootEnd;
        private int animIdStepProgress;
        private bool speedScaleFromPanel;
        private Animator animator;
        private AnimatorMoverCompensator[] compensators;

        public AnimatorMoverBipedStepProgress(Transform transform, float leftFootStart, float leftFootEnd, AnimatorMoverCompensator[] compensators, float speedScale, bool speedScaleFromPanel) : base(transform)
        {
            this.speedScale = speedScale;
            this.leftFootStart = leftFootStart;
            this.leftFootEnd = leftFootEnd;
            this.compensators = compensators;
            this.speedScaleFromPanel = speedScaleFromPanel;
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

        protected override Vector3 GetMoveDelta()
        {
            return base.GetMoveDelta() * speedScale * GetSpeedScaleFromPanel();
        }

        private float GetSpeedCompensation(float stepPreogress)
        {
            var compensation = 0f;
            for(int i = 0; i < compensators.Length; i++)
            {
                compensation += compensators[i].GetCompensation(stepPreogress);
            }
            return compensation;
        }

        private float GetSpeedScaleFromPanel()
        {
            if(speedScaleFromPanel)
            {
                return GetRunSpeedScale();
            }
            return 1;
        }
    }
}