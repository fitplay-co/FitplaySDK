using UnityEngine;

namespace SEngineCharacterController
{
    public class StateMachine<T> : IStateMachine<T> where T : class
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        public IState<T> CurrentState
        {
            get;
            private set;
        }

        /// <summary>
        /// 上个状态
        /// </summary>
        public IState<T> PreState
        {
            get;
            private set;
        }
        
        public StateMachine(IState<T> initialState)
        {
            Initialize(initialState);
        }

        public void ChangeState(IState<T> nextState)
        {
            if (nextState == CurrentState) return;
            CurrentState?.Exit();
            PreState = CurrentState;
            CurrentState = nextState;
            CurrentState?.Enter();
        }

        public void OnTick(float deltaTime)
        {
            if(CurrentState == null) return;
            if(CurrentState.IsTermination) return;
            CurrentState.Tick(deltaTime);
            var nextState = CurrentState.Event();
            ChangeState(nextState);
        }

        public void OnTermination(IState<T> state)
        {
            state.IsTermination = true;
        }

        private void Initialize(IState<T> initialState)
        {
            if (initialState == null)
            {
                Debug.LogError($"状态不存在{typeof(T)}");
                return;
            }

            PreState = null;
            CurrentState = initialState;
            initialState.Enter();
        }
    }
}