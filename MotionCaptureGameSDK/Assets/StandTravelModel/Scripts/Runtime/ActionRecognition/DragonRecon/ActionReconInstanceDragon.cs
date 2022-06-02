using UnityEngine;

public class ActionReconInstanceDragon : ActionReconInstance
{
    private float lastEvent;
    private ActionId actionId;
    private OnActionDetect onActionDetect;

    public ActionReconInstanceDragon(OnActionDetect onActionDetect) : base()
    {
        this.onActionDetect = onActionDetect;

        recognizer.AddRecon(new ActionReconArmExtendHorizon(true, OnActionRecon, false));
        recognizer.AddRecon(new ActionReconArmExtendHorizon(false, OnActionRecon, false));
        recognizer.AddRecon(new ActionReconArmExtendVerticle(true, OnActionRecon, false));
        recognizer.AddRecon(new ActionReconArmExtendVerticle(false, OnActionRecon, false));
    }

    public override ActionId GetActionId()
    {
        if(Time.time < lastEvent + 3)
        {
            return actionId;
        }
        return ActionId.None;
    }

    private void OnActionRecon(ActionId actionId)
    {
        this.actionId = actionId;
        this.lastEvent = Time.time;

        if(onActionDetect != null)
        {
            onActionDetect(actionId);
        }
    }
}