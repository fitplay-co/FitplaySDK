using StandTravelModel.Scripts.Runtime.Mover;
using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

public class AnimatorMoverStepBehaviourFixedSpeed : AnimatorMoverStepBehaviour
{
    [SerializeField] private float speed;

    protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
    {
        return new AnimatorMoverOSSpeed(GetSpeed, animator.transform);
    }

    protected float GetSpeed()
    {
        return Mathf.Min(speed, 10);
    }
}