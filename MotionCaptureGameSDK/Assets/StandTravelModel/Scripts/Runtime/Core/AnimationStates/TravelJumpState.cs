using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
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
                //TODO: implement jump
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