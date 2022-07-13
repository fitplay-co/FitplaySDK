using UnityEngine;

public class ActionReconInstanceHuman : ActionReconInstance
{
    public ActionReconInstanceHuman(OnActionDetect onActionDetect, bool isDebug) : base(onActionDetect)
    {
        AddRecon(new ActionReconLegUp(true, OnActionReconLeft));
        AddRecon(new ActionReconLegUp(false, OnActionReconRight));
        AddRecon(new ActionReconLegDown(true, OnActionReconLeft));
        AddRecon(new ActionReconLegDown(false, OnActionReconRight));
        AddRecon(new ActionReconLegIdle(true, OnActionReconLeft));
        AddRecon(new ActionReconLegIdle(false, OnActionReconRight));

        SetDebug(isDebug);
    }

    private void OnActionReconLeft(ActionId actionId)
    {
        OnActionRecon(actionId);
    }

    private void OnActionReconRight(ActionId actionId)
    {
        OnActionRecon(actionId);
    }
}