using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover
{
    public class AnimatorMoverStepBehaviourDualFoot : AnimatorMoverStepBehaviour
    {
        [SerializeField] private bool speedScaleFromPanel;
        [SerializeField] private float speedScale;
        [SerializeField] private float progressLeftStart;
        [SerializeField] private float progressLeftEnd;
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private AnimatorMoverCompensator[] compensators;

        protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
        {
            return new AnimatorMoverBipedStepProgress(animator.transform, progressLeftStart, progressLeftEnd, compensators, speedScale, speedScaleFromPanel);
        }
    }
}