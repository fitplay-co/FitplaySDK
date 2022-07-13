using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers.Arm
{
    public abstract class ActionReconArmExtend : ActionReconArm
    {
        public ActionReconArmExtend(bool isLeft, Vector2 anglesElbow, Vector2 anglesShoulder, Action<ActionId> onAction) : base (isLeft, anglesElbow, anglesShoulder, onAction)
        {
        }

        protected bool IsRaising()
        {
            return GetUpperarmAngle() < 90;
        }
    }
}