using System.Collections.Generic;
using UnityEngine;

public class ActionReconInstance : IActionReconInstance
{
    private ActionId lastActionId;
    private OnActionDetect onActionDetect;
    private ActionRecognizer recognizer;

    public ActionReconInstance(OnActionDetect onActionDetect)
    {
        this.recognizer = new ActionRecognizer();
        this.onActionDetect = onActionDetect;
    }

    public virtual ActionId GetActionId()
    {
        return lastActionId;
    }

    public void OnUpdate(List<Vector3> keyPoints)
    {
        if(keyPoints != null)
        {
            recognizer.OnUpdate(keyPoints);
        }
    }

    public void SetDebug(bool isDebug)
    {
        recognizer.SetDebug(isDebug);
    }

    protected void AddRecon(IActionRecon recon)
    {
        recognizer.AddRecon(recon);
    }

    protected virtual void OnActionRecon(ActionId actionId)
    {
        lastActionId = actionId;
        if(onActionDetect != null)
        {
            onActionDetect(actionId);
        }
    }
}