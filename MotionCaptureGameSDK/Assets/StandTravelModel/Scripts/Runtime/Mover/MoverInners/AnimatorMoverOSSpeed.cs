using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public class AnimatorMoverOSSpeed : AnimatorMoverFixedSpeed
    {
        protected StandTravelModelManager standTravelModelManager;

        public AnimatorMoverOSSpeed(Func<float> getSpeed, Transform transform) : base(getSpeed, transform)
        {
            standTravelModelManager = transform.GetComponent<StandTravelModelManager>();
        }

        protected override float GetSpeed()
        {
            return base.GetSpeed() * GetOSVelocity();
        }

        protected virtual float GetOSVelocity()
        {
            if(standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.leftLeg != 0 || standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.rightLeg != 0)
            {
                return standTravelModelManager.motionDataModelReference.GetActionDetectionData().walk.velocity;
            }
            return 0;
        }
    }
}