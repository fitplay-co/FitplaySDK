using UnityEngine;

namespace SEngineCharacterController
{
    public class IdleState : State<FsmComponent>
    {
        public IdleState(FsmComponent owner) : base(owner)
        {
            
        }
        
        public override void Enter()
        {
            
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override IState<FsmComponent> Event()
        {
            
            return base.Event();
        }

        public override void Exit()
        {
            
            base.Exit();
        }
    }
}