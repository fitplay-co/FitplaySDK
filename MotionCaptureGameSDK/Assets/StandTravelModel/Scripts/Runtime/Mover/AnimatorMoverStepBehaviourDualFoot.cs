using UnityEngine;

public class AnimatorMoverStepBehaviourDualFoot : AnimatorMoverStepBehaviour
{
    protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
    {
        return new AnimatorMoverBiped(animator.transform);
    }
}