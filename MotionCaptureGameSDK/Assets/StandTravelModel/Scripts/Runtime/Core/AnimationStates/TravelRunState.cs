using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelRunState : AnimationStateBase
    {

        
        public TravelRunState(MotionModelBase owner) : base(owner)
        {
            InitFields(AnimationList.Run);
        }

        public override void Enter()
        {
            //Debug.Log("TravelRunState:Enter");
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            var actionDetectionData = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if (actionDetectionData.jump != null)
            {
                Debug.LogError($"Leg: {actionDetectionData.jump.up}, Strength: {actionDetectionData.jump.strength}");
                if (actionDetectionData.jump.up == 1)
                {
                    travelOwner.ChangeState(AnimationList.Jump);
                    return;
                }
            }

            if (actionDetectionData.walk != null)
            {
                Debug.LogError($"Leg: {actionDetectionData.walk.legUp}, Frequency: {actionDetectionData.walk.frequency}, Strength: {actionDetectionData.walk.strength}");
                travelOwner.EnqueueStep(actionDetectionData.walk.legUp);
                travelOwner.currentLeg = actionDetectionData.walk.legUp;
                travelOwner.currentFrequency = actionDetectionData.walk.frequency / 60f;
                travelOwner.UpdateAnimatorCadence();
                
                if (actionDetectionData.walk.legUp == 0)
                {
                    travelOwner.ChangeState(AnimationList.Idle);
                    return;
                }
            }
        }

        public override void Exit()
        {
            //Debug.Log("TravelRunState:Exit");
            base.Exit();
        }
    }
}