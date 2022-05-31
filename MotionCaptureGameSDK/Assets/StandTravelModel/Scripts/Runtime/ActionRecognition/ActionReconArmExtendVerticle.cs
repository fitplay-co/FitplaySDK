using System;
using UnityEngine;

public class ActionReconArmExtendVerticle : ActionReconArmExtend
{
    public ActionReconArmExtendVerticle(bool isLeft, Action<ActionId> onAction, bool isDebug) : base(isLeft, onAction, isDebug)
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

    protected override bool InConditionArea()
    {
        return IsForearmExtend() && IsUpperarmRaised();
    }
}