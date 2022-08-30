using System;
using StandTravelModel.Scripts.Runtime;
using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public class AnimatorMoverOSSpeedWithSprint : AnimatorMoverOSSpeed
    {
        private Func<float> sprintScale;
        private RunConditioner runConditioner;

        public AnimatorMoverOSSpeedWithSprint(Func<float> sprintScale, Func<float> getSpeed, Transform transform) : base(getSpeed, transform)
        {
            this.sprintScale = sprintScale;
        }

        protected override float GetOSVelocity()
        {
            if(runConditioner == null)
            {
                runConditioner = standTravelModelManager.GetRunConditioner();
            }

            var velocity = base.GetOSVelocity();
            if(runConditioner != null && runConditioner.IsEnterSprintReady(standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk))
            {
                velocity *= sprintScale();
            }

            return velocity;
        }
    }
}