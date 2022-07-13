using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconUpdater;
using StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers.Arm;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.DragonRecon
{
    public class ActionReconInstanceDragon : ActionReconInstance.ActionReconInstance
    {
        private float lastEvent;

        public ActionReconInstanceDragon(OnActionDetect onActionDetect) : base(onActionDetect)
        {
            AddRecon(new ActionReconArmExtendHorizon(true, OnActionRecon, false));
            AddRecon(new ActionReconArmExtendHorizon(false, OnActionRecon, false));
            AddRecon(new ActionReconArmExtendVerticle(true, OnActionRecon, false));
            AddRecon(new ActionReconArmExtendVerticle(false, OnActionRecon, false));
        }

        public override ActionId GetActionId()
        {
            if(Time.time < lastEvent + 3)
            {
                return base.GetActionId();
            }
            return ActionId.None;
        }

        protected override void OnActionRecon(ActionId actionId)
        {
            this.lastEvent = Time.time;
            base.OnActionRecon(actionId);
        }
    }
}