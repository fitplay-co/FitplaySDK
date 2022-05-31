using UnityEngine;

public class ActionReconUpdaterDragon : ActionReconUpdater
{
    protected override IActionReconInstance CreateReconInstance()
    {
        return new ActionReconInstanceDragon();
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
}