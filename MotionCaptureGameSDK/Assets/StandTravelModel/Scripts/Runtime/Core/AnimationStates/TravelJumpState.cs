using MotionCaptureBasic.FSM;
using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelJumpState : State<TravelModel>
    {
        public TravelJumpState(TravelModel owner) : base(owner)
        {
            
        }

        public override void Enter()
        {
            Debug.Log("TravelJumpState:Enter");
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("TravelJumpState:Exit");
        }
    }
}