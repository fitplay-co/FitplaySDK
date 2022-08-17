using StandTravelModel.Scripts.Runtime;
using StandTravelModel.Scripts.Runtime.Mover;
using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

public class AnimatorMoverStepBehaviourFixedSpeed : AnimatorMoverStepBehaviour
{
    [SerializeField] private float speed;

    private StandTravelModelManager standTravelModelManager;

    protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
    {
        standTravelModelManager = animator.GetComponent<StandTravelModelManager>();
        return new AnimatorMoverFixedSpeed(() => speed * GetOSVelocity(), animator.transform);
    }

    private float GetOSVelocity()
    {
        return standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.velocity;
    }
}