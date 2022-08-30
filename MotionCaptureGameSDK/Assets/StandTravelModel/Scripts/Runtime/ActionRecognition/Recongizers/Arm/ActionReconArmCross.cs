using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers.Arm
{
    public class ActionReconArmCross : ActionReconArmExtend
    {
        protected const float elbowAngleMin = 40;
        protected const float elbowAngleMax = 55;
        protected const float shoulderAngleMin = 0;
        protected const float shoulderAngleMax = 25;

        public ActionReconArmCross(bool isLeft, Action<ActionId> onAction, bool isDebug) : base(
            isLeft,
            new Vector2(elbowAngleMin, elbowAngleMax),
            new Vector2(shoulderAngleMin, shoulderAngleMax),
            onAction
        )
        {
        }

        protected override ActionId GetActionIdLeft()
        {
            return ActionId.ArmCrossLeft;
        }

        protected override ActionId GetActionIdRight()
        {
            return ActionId.ArmCrossRight;
        }
    }
}