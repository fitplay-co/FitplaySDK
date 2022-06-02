using UnityEngine;

public delegate void OnActionDetect(ActionId actionId);

public class ActionReconUpdaterDragon : ActionReconUpdater
{
    public event OnActionDetect onActionDetect;

    protected override IActionReconInstance CreateReconInstance()
    {
        return new ActionReconInstanceDragon(OnActionDetect);
    }

    private void OnGUI() {
        var actionId = GetActionId();
        if(actionId != ActionId.None)
        {
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 25;
            labelStyle.normal.textColor = Color.green;
            GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.4f, 300, 80), "事件:" + actionId, labelStyle);
        }
    }

    private void OnActionDetect(ActionId actionId)
    {
        if(onActionDetect != null)
        {
            onActionDetect(actionId);
        }
    }
}