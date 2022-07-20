using UnityEngine;

namespace SEngineCharacterController
{
    public class DieState : State<FsmComponent>
    {
        public DieState(FsmComponent owner) : base(owner)
        {
            
        }
        
        public override void Enter()
        {
            Debug.LogError("DieState:Enter");
            Debug.LogError($"{Owner.StateMachine.PreState} -- { Owner.StateMachine.CurrentState}" );
        }

        public override void Tick(float deltaTime)
        {
//            Debug.LogError("DieState:Tick");
        }

        public override IState<FsmComponent> Event()
        {
            return base.Event();
        }

        public override void Exit()
        {
            Debug.LogError("DieState:Exit");
        }
    }
}