using System;
using UnityEngine;

public class ActionReconArmExtendVerticle : ActionReconArmExtend
{
    protected const float elbowAngleMin = 0;
    protected const float elbowAngleMax = 25;
    protected const float shoulderAngleMin = 0;
    protected const float shoulderAngleMax = 25;

    public ActionReconArmExtendVerticle(bool isLeft, Action<ActionId> onAction, bool isDebug) : base(
        isLeft,
        new Vector2(elbowAngleMin, elbowAngleMax),
        new Vector2(shoulderAngleMin, shoulderAngleMax),
        onAction
        )
    {
    }

    protected override ActionId GetActionIdLeft()
    {
        return IsRaising() ? ActionId.ArmExtensionLeftUp : ActionId.ArmExtensionLeftDown;
    }

    protected override ActionId GetActionIdRight()
    {
        return IsRaising() ? ActionId.ArmExtensionRightUp : ActionId.ArmExtensionRightDown;
    }
}