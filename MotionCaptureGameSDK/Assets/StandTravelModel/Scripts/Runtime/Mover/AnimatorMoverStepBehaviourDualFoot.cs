using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover
{
    public class AnimatorMoverStepBehaviourDualFoot : AnimatorMoverStepBehaviour
    {
        [SerializeField] private float progressLeftStart;
        [SerializeField] private float progressLeftEnd;

        protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
        {
            return new AnimatorMoverBipedStepProgress(animator.transform, progressLeftStart, progressLeftEnd);
        }
    }
}