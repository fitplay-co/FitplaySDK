using AnimationUprising.Strider;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

public class TravelStrideSetter : ITravelStrideSetter
{
    private TravelModel travelOwner;
    private StriderBiped striderBiped;
    private ActionDetectionItem actionDetectionItem;

    public TravelStrideSetter(StriderBiped striderBiped, TravelModel travelOwner)
    {
        this.travelOwner = travelOwner;
        this.striderBiped = striderBiped;
    }

    public void UpdateSpeedScale()
    {
        actionDetectionItem = travelOwner.selfMotionDataModel.GetActionDetectionData();
        if(actionDetectionItem != null && actionDetectionItem.walk != null)
        {
            /* var leftStepLength = actionDetectionItem.walk.leftStepLength * 0.7f;
            var rightStepLength = actionDetectionItem.walk.rightStepLength * 0.7f; */

            //striderBiped.SpeedScale = Mathf.Max(leftStepLength, 0.25f);
            //striderBiped.SpeedScale = 1;
            if(actionDetectionItem.walk.realtimeLeftLeg == 1)
            {
                //striderBiped.SpeedScale = GetStride(actionDetectionItem.walk.leftHipAng);
            }
        }
    }

    private float GetStride(float hipAngle)
    {
        var stride = (180f - hipAngle) / 60;
        return Mathf.Max(0.3f, stride);
    }
}