using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers.Arm
{
    public class ActionReconArmExtendHorizon : ActionReconArmExtend
    {
        protected const float elbowAngleMin = 0;
        protected const float elbowAngleMax = 25;
        protected const float shoulderAngleMin = 75;
        protected const float shoulderAngleMax = 105;

        public ActionReconArmExtendHorizon(bool isLeft, Action<ActionId> onAction, bool isDebug) : base(
            isLeft,
            new Vector2(elbowAngleMin, elbowAngleMax),
            new Vector2(shoulderAngleMin, shoulderAngleMax),
            onAction
        )
        {
        }

        protected override ActionId GetActionIdLeft()
        {
            return ActionId.ArmExtensionLeft;
        }

        protected override ActionId GetActionIdRight()
        {
            return ActionId.ArmExtensionRight;
        }
    }
}