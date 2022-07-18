using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconInstance;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconUpdater;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.DragonRecon
{
    public class ActionReconUpdaterDragon : ActionReconUpdater.ActionReconUpdater
    {
        protected override IActionReconInstance CreateReconInstance(OnActionDetect onActionDetect)
        {
            return new ActionReconInstanceDragon(onActionDetect);
        }
    }
}