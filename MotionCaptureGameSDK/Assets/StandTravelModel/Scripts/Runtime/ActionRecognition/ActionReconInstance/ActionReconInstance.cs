using System.Collections.Generic;
using UnityEngine;

public class ActionReconInstance : IActionReconInstance
{
    protected ActionRecognizer recognizer;

    public ActionReconInstance()
    {
        recognizer = new ActionRecognizer();
    }

    public virtual ActionId GetActionId()
    {
        return ActionId.None;
    }

    public void OnUpdate(List<Vector3> keyPoints)
    {
        recognizer.OnUpdate(keyPoints);
    }
}