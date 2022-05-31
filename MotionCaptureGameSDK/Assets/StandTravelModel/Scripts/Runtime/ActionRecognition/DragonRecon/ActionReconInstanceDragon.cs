using UnityEngine;

public class ActionReconInstanceDragon : ActionReconInstance
{
    private float lastEvent;
    private ActionId actionId;

    public ActionReconInstanceDragon() : base()
    {
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
    }
}