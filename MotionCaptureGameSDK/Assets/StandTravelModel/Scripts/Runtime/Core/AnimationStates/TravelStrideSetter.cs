using AnimationUprising.Strider;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.MotionModel;

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
            UnityEngine.Debug.Log("actionDetectionItem.walk.leftStepLength " + actionDetectionItem.walk.leftStepLength + "|" + actionDetectionItem.walk.rightStepLength);
            striderBiped.SpeedScale = 1; 
        }
    }
}