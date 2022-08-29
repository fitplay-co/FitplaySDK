using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover
{
    public abstract class AnimatorMoverStepBehaviour : StateMachineBehaviour
    {
        private IAnimatorMoverBiped animMover;

        protected abstract IAnimatorMoverBiped CreateAnimatorMover(Animator animator);

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animMover = CreateAnimatorMover(animator);
            animMover.OnStart();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            animMover.OnUpdate(stateInfo);
        }

        protected void SetAnimatorMover(IAnimatorMoverBiped animatorMover)
        {
            this.animMover = animatorMover;
        }
    }
}