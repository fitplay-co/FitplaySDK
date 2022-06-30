using System;

public abstract class ActionReconLimb : ActionReconBase
{
    private ActionId curActionId;

    public ActionReconLimb(bool isLeft, Action<ActionId> onAction) : base(isLeft, onAction)
    {
    }

    protected abstract ActionId GetActionIdLeft();
    protected abstract ActionId GetActionIdRight();


    protected override void OnAction(bool active)
    {
        var actionId = ActionId.None;
        if(active)
        {
            actionId = IsLeft() ? GetActionIdLeft() : GetActionIdRight();
            SendAction(actionId);
        }
        
        /* if(curActionId != actionId)
        {
            SendAction(curActionId);
        } */

        curActionId = actionId;
        //SendAction(actionId);
    }
}