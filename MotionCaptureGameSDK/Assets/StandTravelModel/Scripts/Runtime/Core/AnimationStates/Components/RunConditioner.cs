using MotionCaptureBasic.OSConnector;

namespace StandTravelModel.Core.AnimationStates
{
    public class RunConditioner
    {
        public bool IsEnterRunReady(WalkActionItem walkData)
        {
            //return (walkData.leftFrequency * walkData.leftStepLength > 5) || (walkData.rightFrequency * walkData.rightStepLength > 5);
            return walkData.leftFrequency > 5 || walkData.rightFrequency > 5;
        }
    }
}