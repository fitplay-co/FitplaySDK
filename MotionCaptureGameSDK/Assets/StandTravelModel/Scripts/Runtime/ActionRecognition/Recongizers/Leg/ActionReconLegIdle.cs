using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers.Leg
{
    public class ActionReconLegIdle : ActionReconLeg
    {
        private const float idleAngle = 50;

        public ActionReconLegIdle(bool isLeft, Action<ActionId> onAction) : base(
            isLeft,
            true,
            new Vector2(0, idleAngle),
            new Vector2(180 - idleAngle, 180),
            onAction
        )
        {
        }

        protected override ActionId GetActionIdLeft()
        {
            return ActionId.LegIdleLeft;
        }

        protected override ActionId GetActionIdRight()
        {
            return ActionId.LegIdleRight;
        }
    }
}