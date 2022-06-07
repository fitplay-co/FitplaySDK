using MotionCaptureBasic.FSM;
using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelSquatState : State<TravelModel>
    {
        public TravelSquatState(TravelModel owner) : base(owner)
        {
            
        }

        public override void Enter()
        {
            Debug.Log("TravelSquatState:Enter");
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("TravelSquatState:Exit");
        }
    }
}