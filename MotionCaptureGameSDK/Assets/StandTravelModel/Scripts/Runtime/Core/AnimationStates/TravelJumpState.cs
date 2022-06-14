using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelJumpState : AnimationStateBase
    {

        public TravelJumpState(MotionModelBase owner) : base(owner)
        {
            InitFields(AnimationList.Jump);
        }

        public override void Enter()
        {
            //Debug.Log("TravelJumpState:Enter");
            travelOwner.isJump = true;
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            var actionDetectionData = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if (actionDetectionData.jump != null)
            {
                Debug.LogError($"Leg: {actionDetectionData.jump.up}, Strength: {actionDetectionData.jump.strength}");
                if (actionDetectionData.jump.up == 0)
                {
                    travelOwner.ChangePrevState();
                    return;
                }
            }
        }

        public override void Exit()
        {
            //Debug.Log("TravelJumpState:Exit");
            travelOwner.isJump = false;
            base.Exit();
        }
    }
}