using System.Collections.Generic;

namespace SEngineCharacterController
{
    public class FsmComponent : Component
    {
        public StateMachine<FsmComponent> StateMachine { private set; get; }
        public Dictionary<CharacterState, State<FsmComponent>> States { private set; get; }

        public override void OnInit()
        {
            States = new Dictionary<CharacterState, State<FsmComponent>>
            {
                {CharacterState.Idle, new IdleState(this)}, 
                {CharacterState.Die, new DieState(this)}, 
            };
            
            StateMachine = new StateMachine<FsmComponent>(States[CharacterState.Idle]);
            
            Launcher.Instance.RegisterFixedTick(OnFixedTick);
            base.OnInit();
        }

        private void OnFixedTick(float dt)
        {
            StateMachine.OnTick(dt);
        }

        public void ChangeState(State<FsmComponent> nextState)
        {
            StateMachine.ChangeState(nextState);
        }
    }
}