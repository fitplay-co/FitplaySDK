using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover
{
    public class AnimatorMoverStepBehaviourDualFoot : AnimatorMoverStepBehaviour
    {
        [SerializeField] private bool needPause;
        [SerializeField] private bool speedScaleFromPanel;
        [SerializeField] private float speedScale;
        [SerializeField] private float progressLeftStart;
        [SerializeField] private float progressLeftEnd;
        [SerializeField] private AnimatorMoverCompensator[] compensators;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);
            if(needPause)
            {
                Debug.Break();
            }
        }

        protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
        {
            return new AnimatorMoverBipedStepProgress(animator.transform, progressLeftStart, progressLeftEnd, compensators, speedScale, speedScaleFromPanel);
        }
    }
}