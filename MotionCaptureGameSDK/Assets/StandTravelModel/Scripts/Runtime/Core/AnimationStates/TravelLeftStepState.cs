using MotionCaptureBasic.FSM;
using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelLeftStepState : State<TravelModel>
    {
        public TravelLeftStepState(TravelModel owner) : base(owner)
        {
            
        }

        public override void Enter()
        {
            Debug.Log("TravelLeftStepState:Enter");
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("TravelLeftStepState:Exit");
        }
    }
}