using MotionCaptureBasic;
using MotionCaptureBasic.OSConnector;
using UnityEngine;
using StandTravelModel;

public class ActionEventDisplayer : MonoBehaviour
{
    [SerializeField] private Animator charAnim;

    private int leftCount;
    private int rightCount;
    private ActionId leftId;
    private ActionId rightId;
    private ActionId leftBack;
    private ActionId rightBack;
    private ActionDetectionItem actionDetection;
    private StandTravelModelManager standTravelManager;

    private void OnGUI() {
        actionDetection = MotionDataModelHttp.GetInstance().GetActionDetectionData();

        GetActionIds(out leftId, out rightId);

        if(leftId != leftBack)
        {
            leftBack = leftId;
            leftCount++;
        }

        if(rightId != rightBack)
        {
            rightBack = rightId;
            rightCount++;
        }

        if(leftId != ActionId.None)
        {
            DrawEnvent(leftId, leftCount);
        }

        if(rightId != ActionId.None)
        {
            DrawEnvent(rightId, rightCount);
        }

        DrawHipAngles();

        DrawLegProgress("progressUpLeft", 0.1f, true);
        DrawLegProgress("progressDownLeft", 0.3f, false);
        DrawStepProgress();
    }

    private void DrawEnvent(ActionId actionId, int count)
    {
        GUIStyle labelStyle = new GUIStyle("label");
        labelStyle.fontSize = 35;
        labelStyle.normal.textColor = Color.green;

        var x = 0f;
        var y = 0f;
        GetScreenPos(out x, out y, actionId);
        GUI.Label(new Rect(x, y, 300, 80), "事件:" + actionId, labelStyle);

        if(ActionId.LegUpLeft == actionId || ActionId.LegDownLeft == actionId || ActionId.LegIdleLeft == actionId)
        {
            GUI.Label(new Rect(0.1f * Screen.width, 0.7f * Screen.height, 300, 80), count.ToString(), labelStyle);
        }
        else if(ActionId.LegUpRight == actionId || ActionId.LegDownRight == actionId || ActionId.LegIdleRight == actionId)
        {
            GUI.Label(new Rect(0.9f * Screen.width, 0.7f * Screen.height, 300, 80), count.ToString(), labelStyle);
        }
    }

    private void DrawHipAngles()
    {
        if(actionDetection != null && actionDetection.walk != null)
        {
            DrawHipAngle(true, actionDetection.walk.leftHipAng);
            DrawHipAngle(false, actionDetection.walk.rightHipAng);
        }
    }

    private void DrawHipAngle(bool isLeft, float angle)
    {
        GUIStyle labelStyle = new GUIStyle("label");
        labelStyle.fontSize = 35;
        labelStyle.normal.textColor = Color.blue;

        var x = isLeft ? 0.2f : 0.8f;
        var y = 0.5f * angle / 90f;

        x *= Screen.width;
        y *= Screen.height;

        GUI.Label(new Rect(x, y, 300, 80), "角度:" + angle, labelStyle);

        GUI.Label(new Rect(x, Screen.height * 0.50f, 300, 80), "90", labelStyle);
        GUI.Label(new Rect(x, Screen.height * 0.75f, 300, 80), "135", labelStyle);
        GUI.Label(new Rect(x, Screen.height * 1.00f, 300, 80), "180", labelStyle);
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

    private void GetActionIds(out ActionId left, out ActionId right)
    {
        left = ActionId.None;
        right = ActionId.None;

        if(actionDetection != null && actionDetection.walk != null)
        {
            if(actionDetection.walk.leftLeg == -1)
            {
                left = ActionId.LegDownLeft;
            }

            if(actionDetection.walk.leftLeg == 1)
            {
                left = ActionId.LegUpLeft;
            }

            if(actionDetection.walk.leftLeg == 0)
            {
                left = ActionId.LegIdleLeft;
            }

            if(actionDetection.walk.rightLeg == -1)
            {
                right = ActionId.LegDownRight;
            }

            if(actionDetection.walk.rightLeg == 1)
            {
                right = ActionId.LegUpRight;
            }

            if(actionDetection.walk.rightLeg == 0)
            {
                right = ActionId.LegIdleRight;
            }
        }
    }

    private void DrawLegProgress(string id, float x, bool inverse)
    {
        var progress = charAnim.GetFloat(id);
        var proValue = progress;

        if(inverse)
        {
            progress = 1 - progress;
        }        

        GUIStyle labelStyle = new GUIStyle("label");
        labelStyle.fontSize = 25;
        labelStyle.normal.textColor = Color.yellow;

        x *= Screen.width;
        var y = Screen.height * (progress * 0.5f + 0.5f);

        GUI.Label(new Rect(x, y, 300, 80), id + "->" + proValue, labelStyle);
    }

    private void DrawStepProgress()
    {
        if(standTravelManager == null)
        {
            standTravelManager = charAnim.GetComponent<StandTravelModelManager>();
        }

        var stepProgress = standTravelManager.stepSmoother.GetStepProgress();
        var targetProgress = standTravelManager.stepSmoother.GetTargetProgress();

        GUIStyle labelStyle = new GUIStyle("label");
        labelStyle.fontSize = 32;
        labelStyle.normal.textColor = Color.green;

        var x = Screen.width * stepProgress;
        var y = Screen.height * 0.8f;
        GUI.Label(new Rect(x, y, 300, 80), "|||", labelStyle);

        labelStyle.normal.textColor = Color.red;
        x = Screen.width * stepProgress;
        y = Screen.height * 0.9f;
        GUI.Label(new Rect(x, y, 300, 80), "|||", labelStyle);
    }
}