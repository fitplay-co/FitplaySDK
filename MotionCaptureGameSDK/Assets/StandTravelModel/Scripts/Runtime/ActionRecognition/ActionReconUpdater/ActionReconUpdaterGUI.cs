using UnityEngine;

public partial class ActionReconUpdater : MonoBehaviour
{
    private ActionId leftId;
    private ActionId rightId;

    private void OnGUI() {
        var actionId = GetActionId();
        if(actionId != ActionId.None)
        {
            if(
                actionId == ActionId.LegUpLeft ||
                actionId == ActionId.LegDownLeft ||
                actionId == ActionId.LegIdleLeft
            ) {
                leftId = actionId;
            }
            
            if(
                actionId == ActionId.LegUpRight ||
                actionId == ActionId.LegDownRight ||
                actionId == ActionId.LegIdleRight
            ) {
                rightId = actionId;
            }
        }

        if(leftId != ActionId.None)
        {
            DrawEnvent(leftId);
        }

        if(rightId != ActionId.None)
        {
            DrawEnvent(rightId);
        }
    }

    private void DrawEnvent(ActionId actionId)
    {
        GUIStyle labelStyle = new GUIStyle("label");
        labelStyle.fontSize = 25;
        labelStyle.normal.textColor = Color.green;

        var x = 0f;
        var y = 0f;
        GetScreenPos(out x, out y, actionId);
        GUI.Label(new Rect(x, y, 300, 80), "事件:" + actionId, labelStyle);
    }

    private void GetScreenPos(out float x, out float y, ActionId actionId)
    {
        if(actionId == ActionId.LegUpLeft)
        {
            x = 0.2f;
            y = 0.3f;
        }
        else if(actionId == ActionId.LegUpRight)
        {
            x = 0.8f;
            y = 0.3f;
        }
        else if(actionId == ActionId.LegDownLeft)
        {
            x = 0.2f;
            y = 0.6f;
        }
        else if(actionId == ActionId.LegDownRight)
        {
            x = 0.8f;
            y = 0.6f;
        }
        else if(actionId == ActionId.LegIdleLeft)
        {
            x = 0.1f;
            y = 0.9f;
        }
        else if(actionId == ActionId.LegIdleRight)
        {
            x = 0.9f;
            y = 0.9f;
        }
        else
        {
            x = 0;
            y = 0;    
        }

        x *= Screen.width;
        y *= Screen.height;
    }
}