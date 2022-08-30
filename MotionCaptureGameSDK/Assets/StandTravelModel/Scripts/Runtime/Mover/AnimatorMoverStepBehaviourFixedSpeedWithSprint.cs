using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using UnityEngine;

public class AnimatorMoverStepBehaviourFixedSpeedWithSprint : AnimatorMoverStepBehaviourFixedSpeed
{
    [SerializeField] private float speedScale = 1.5f;

    private RunConditioner runConditioner;

    protected override float GetOSVelocity()
    {
        if(runConditioner == null)
        {
            runConditioner = standTravelModelManager.GetRunConditioner();
        }

        var velocity = base.GetOSVelocity();
        if(runConditioner != null && runConditioner.IsEnterSprintReady(standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk))
        {
            velocity *= speedScale;
        }

        return velocity;
    }
}