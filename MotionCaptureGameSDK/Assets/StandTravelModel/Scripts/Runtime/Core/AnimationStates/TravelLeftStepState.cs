using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelLeftStepState : AnimationStateBase
    {


        public TravelLeftStepState(MotionModelBase owner) : base(owner)
        {
            InitFields(AnimationList.LeftStep);
        }

        public override void Enter()
        {
            //Debug.Log("TravelLeftStepState:Enter");
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
                
                var isRunReady = travelOwner.IsEnterRunReady();
                if (actionDetectionData.walk.legUp != 0 && isRunReady)
                {
                    travelOwner.ChangeState(AnimationList.Run);
                    return;
                }

                if (actionDetectionData.walk.legUp == -1)
                {
                    travelOwner.ChangeState(AnimationList.RightStep);
                    return;
                }
            }
        }

        public override void Exit()
        {
            //Debug.Log("TravelLeftStepState:Exit");
            base.Exit();
        }
    }
}