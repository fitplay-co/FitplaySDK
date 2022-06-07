using UnityEngine;

namespace MotionCaptureBasic.FSM
{
    public class StateMachine<TOwner> : IStateMachine<TOwner> where TOwner : class
    {
        private State<TOwner> _currentState; // 当前state
        private State<TOwner> _prevState;    // 前一个state

        public StateMachine()
        {
            
        }
        
        public StateMachine(State<TOwner> initialState)
        {
            Initialize(initialState);
        }

        public void ChangeState(State<TOwner> state)
        {
            if (state == null) return;
            if(_currentState == state) return;
            _prevState = _currentState;
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void ChangePrevState()
        {
            if (_prevState == null)
            {
                Debug.LogError($"前一个状态不存在");
                return;
            }

            ChangeState(_prevState);
        }

        public void OnTick(float deltaTime)
        {
            _currentState?.Tick(deltaTime);
        }
        
        public void Initialize(State<TOwner> initialState)
        {
            if (initialState == null)
            {
                Debug.LogError($"状态不存在{typeof(TOwner)}");
                return;
            }
            _currentState = initialState;
            initialState.Enter();
        }
    }
}