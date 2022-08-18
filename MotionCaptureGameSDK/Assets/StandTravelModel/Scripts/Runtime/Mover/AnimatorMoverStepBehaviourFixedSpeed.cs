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
        if(standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.leftLeg != 0 && standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.rightLeg != 0)
        {
            return standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.velocity;
        }
        return 0;
        //return standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.velocity;
    }
}