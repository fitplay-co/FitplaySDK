using MotionCaptureBasic;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconInstance;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconUpdater;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.HumanRecon
{
    public class ActionReconUpdaterHuman : ActionReconUpdater.ActionReconUpdater
    {
        private ActionDetectionItem simulatActionDetectionItem;
        [UnityEngine.SerializeField] private ActionReconUpdaterHumanMessageFaker humanMessageFaker;

        protected override void Update()
        {
            base.Update();

            if(reconState == ReconState.Fake)
            {
                if(humanMessageFaker == null)
                {
                    humanMessageFaker = new ActionReconUpdaterHumanMessageFaker();
                }

                humanMessageFaker.OnUpdate();
            }
        }

        protected override IActionReconInstance CreateReconInstance(OnActionDetect onActionDetect)
        {
            return new ActionReconInstanceHuman(
                actionId => {
                    onActionDetect(actionId);

                    if(reconState == ReconState.Simulat)
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
                simulatActionDetectionItem.walk.realtimeLeftLeg = -1;
            }

            if(actionId == ActionId.LegUpLeft)
            {
                simulatActionDetectionItem.walk.realtimeLeftLeg = 1;
            }

            if(actionId == ActionId.LegIdleLeft)
            {
                simulatActionDetectionItem.walk.realtimeLeftLeg = 0;
            }

            if(actionId == ActionId.LegDownRight)
            {
                simulatActionDetectionItem.walk.realtimeRightLeg = -1;
            }

            if(actionId == ActionId.LegUpRight)
            {
                simulatActionDetectionItem.walk.realtimeRightLeg = 1;
            }

            if(actionId == ActionId.LegIdleRight)
            {
                simulatActionDetectionItem.walk.realtimeRightLeg = 0;
            }
        }
    }
}