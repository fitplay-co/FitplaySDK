using UnityEngine;
using MotionCaptureBasic;
using MotionCaptureBasic.OSConnector;

public class ActionReconUpdaterHuman : ActionReconUpdater
{
    [SerializeField] private bool simulat;

    private ActionDetectionItem simulatActionDetectionItem;

    protected override IActionReconInstance CreateReconInstance(OnActionDetect onActionDetect)
    {
        return new ActionReconInstanceHuman(
            actionId => {
                onActionDetect(actionId);

                if(simulat)
                {
                    SetSimulatData(actionId);
                }
            },
            false
        );
    }

    private void SetSimulatData(ActionId actionId)
    {
        if(simulatActionDetectionItem == null)
        {
            simulatActionDetectionItem = new ActionDetectionItem();
            simulatActionDetectionItem.walk = new WalkActionItem();
            MotionDataModelHttp.GetInstance().SetSimulatActionDetectionData(simulatActionDetectionItem);
        }

        if(actionId == ActionId.LegDownLeft)
        {
            simulatActionDetectionItem.walk.leftLeg = -1;
        }

        if(actionId == ActionId.LegUpLeft)
        {
            simulatActionDetectionItem.walk.leftLeg = 1;
        }

        if(actionId == ActionId.LegIdleLeft)
        {
            simulatActionDetectionItem.walk.leftLeg = 0;
        }

        if(actionId == ActionId.LegDownRight)
        {
            simulatActionDetectionItem.walk.rightLeg = -1;
        }

        if(actionId == ActionId.LegUpRight)
        {
            simulatActionDetectionItem.walk.rightLeg = 1;
        }

        if(actionId == ActionId.LegIdleRight)
        {
            simulatActionDetectionItem.walk.rightLeg = 0;
        }
    }
}