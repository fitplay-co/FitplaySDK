using StandTravelModel.Scripts.Runtime.MotionModel;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
{
    public class TravelSquatState : AnimationStateBase
    {

        
        public TravelSquatState(MotionModelBase owner) : base(owner)
        {
            InitFields(AnimationList.Squat);
        }

        public override void Enter()
        {
            //Debug.Log("TravelSquatState:Enter");
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            //TODO 等os完善数据
        }

        public override void Exit()
        {
            //Debug.Log("TravelSquatState:Exit");
            base.Exit();
        }
    }
}