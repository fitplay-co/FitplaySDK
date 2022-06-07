using MotionCaptureBasic.FSM;
using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelRightStepState : State<TravelModel>
    {
        public TravelRightStepState(TravelModel owner) : base(owner)
        {
            
        }

        public override void Enter()
        {
            Debug.Log("TravelRightStepState:Enter");
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("TravelRightStepState:Exit");
        }
    }
}