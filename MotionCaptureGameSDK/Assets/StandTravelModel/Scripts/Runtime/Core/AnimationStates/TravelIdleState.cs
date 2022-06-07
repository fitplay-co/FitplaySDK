using MotionCaptureBasic.FSM;
using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelIdleState : State<TravelModel>
    {
        public TravelIdleState(TravelModel owner) : base(owner)
        {

        }

        public override void Enter()
        {
            Debug.Log("TravelIdleState:Enter");
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("TravelIdleState:Exit");
        }
    }
}