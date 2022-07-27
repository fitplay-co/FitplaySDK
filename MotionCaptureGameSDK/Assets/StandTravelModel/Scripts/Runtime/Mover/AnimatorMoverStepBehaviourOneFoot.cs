using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover
{
    public class AnimatorMoverStepBehaviourOneFoot : AnimatorMoverStepBehaviour
    {
        [SerializeField] private int anchorFoot;

        protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
        {
            return new AnimatorMoverAnchorFixed(anchorFoot, animator.transform);
        }
    }
}