using System;

public class ActionReconArmExtendHorizon : ActionReconArmExtend
{
    public ActionReconArmExtendHorizon(bool isLeft, Action<ActionId> onAction, bool isDebug) : base(isLeft, onAction, isDebug)
    {
    }

    protected override bool InConditionArea()
    {
        return IsForearmExtend() && IsUpperarmHorizon();
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