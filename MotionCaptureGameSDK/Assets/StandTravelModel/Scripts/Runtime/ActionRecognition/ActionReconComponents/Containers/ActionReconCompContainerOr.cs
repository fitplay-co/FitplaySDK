using System;

public class ActionReconCompContainerOr : ActionReconCompContainerBase
{
    public ActionReconCompContainerOr(Action<bool> onAction) : base(onAction)
    {
    }

    protected override void OnActionDetect(IActionReconComp comp, bool active)
    {
        SendEvent(active);
    }
}