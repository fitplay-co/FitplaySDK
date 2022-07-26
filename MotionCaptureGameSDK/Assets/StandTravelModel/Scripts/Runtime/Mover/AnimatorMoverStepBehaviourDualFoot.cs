using UnityEngine;

public class AnimatorMoverStepBehaviourDualFoot : AnimatorMoverStepBehaviour
{
    [SerializeField] private float progressLeftStart;
    [SerializeField] private float progressLeftEnd;

    protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
    {
        return new AnimatorMoverBipedStepProgress(animator.transform, progressLeftStart, progressLeftEnd);
    }
}