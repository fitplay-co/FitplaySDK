using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

public class AnimatorMoverStepBehaviourFixedSpeedWithSprint : AnimatorMoverStepBehaviourFixedSpeed
{
    [SerializeField] private float sprintScale = 1.5f;

    protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
    {
        return new AnimatorMoverOSSpeedWithSprint(GetSprintScale, GetSpeed, animator.transform);
    }

    private float GetSprintScale()
    {
        return sprintScale;
    }
}