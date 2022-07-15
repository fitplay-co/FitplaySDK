using UnityEngine;

public class AnimatorMoverStepBehaviourOneFoot : AnimatorMoverStepBehaviour
{
    [SerializeField] private int anchorFoot;

    protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
    {
        return new AnimatorMoverAnchorFixed(anchorFoot, animator.transform);
    }
}