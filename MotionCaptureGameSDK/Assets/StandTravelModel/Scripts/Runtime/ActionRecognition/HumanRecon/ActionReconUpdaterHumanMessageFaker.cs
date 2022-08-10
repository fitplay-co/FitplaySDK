using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

[System.Serializable]
public class ActionReconUpdaterHumanMessageFaker
{
    private enum StageType
    {
        LegLeftUp,
        LegLeftDown,
        LegRightUp,
        LegRightDown,
    }

    [SerializeField] private int stageCount;
    [SerializeField] private float speedUp;
    [SerializeField] private float progress;
    [SerializeField] private float timeLine;
    [SerializeField] private StageType curStage;
    [SerializeField][Range(0, 10)] private float stepFrequency;
    [SerializeField][Range(0, 10)] private float stepLength;

    private ActionDetectionItem simulatActionDetectionItem;

    public ActionReconUpdaterHumanMessageFaker()
    {
        stageCount = System.Enum.GetNames(typeof(StageType)).Length;
    }

    public void OnUpdate(IMotionDataModel motionDataModel)
    {
        if(simulatActionDetectionItem == null)
        {
            simulatActionDetectionItem = new ActionDetectionItem();
            simulatActionDetectionItem.walk = new WalkActionItem();
            //motionDataModel.SetSimulatActionDetectionData(simulatActionDetectionItem);
        }

        timeLine += Time.deltaTime * (speedUp + 1);

        int nextStage = (int)curStage + 1;
        if(timeLine >= (float)(nextStage))
        {
            curStage = (StageType)(nextStage % stageCount);
            OnStageSwitch(curStage);
        }

        progress = timeLine - (int)timeLine;
        timeLine %= stageCount;

        UpdateHipAngle(curStage);
        simulatActionDetectionItem.walk.leftFrequency = stepFrequency;
        simulatActionDetectionItem.walk.leftStepLength = stepLength;
    }

    private void OnStageSwitch(StageType stageType)
    {
        switch(stageType)
        {
            case StageType.LegLeftUp:
            {
                simulatActionDetectionItem.walk.realtimeLeftLeg = 1;
                break;
            }
            case StageType.LegLeftDown:
            {
                simulatActionDetectionItem.walk.realtimeLeftLeg = -1;
                break;
            }
            case StageType.LegRightUp:
            {
                simulatActionDetectionItem.walk.realtimeRightLeg = 1;
                break;
            }
            case StageType.LegRightDown:
            {
                simulatActionDetectionItem.walk.realtimeRightLeg = -1;
                break;
            }
        }
    }

    private void UpdateHipAngle(StageType stageType)
    {
        switch(stageType)
        {
            case StageType.LegLeftUp:
            {
                simulatActionDetectionItem.walk.leftHipAng = GetHipAngle(progress);
                break;
            }
            case StageType.LegLeftDown:
            {
                simulatActionDetectionItem.walk.leftHipAng = GetHipAngle(1f - progress);
                break;
            }
            case StageType.LegRightUp:
            {
                simulatActionDetectionItem.walk.rightHipAng = GetHipAngle(progress);
                break;
            }
            case StageType.LegRightDown:
            {
                simulatActionDetectionItem.walk.rightHipAng = GetHipAngle(1f - progress);
                break;
            }
        }
    }

    private float GetHipAngle(float percent)
    {
        return Mathf.Lerp(150f, 175f, (1f - percent));
    }  
}