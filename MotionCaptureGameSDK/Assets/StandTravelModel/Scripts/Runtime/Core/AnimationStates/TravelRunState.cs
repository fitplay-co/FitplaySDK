using MotionCaptureBasic.FSM;
using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelRunState : State<TravelModel>
    {
        public TravelRunState(TravelModel owner) : base(owner)
        {
            
        }

        public override void Enter()
        {
            Debug.Log("TravelRunState:Enter");
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("TravelRunState:Exit");
        }
    }
}