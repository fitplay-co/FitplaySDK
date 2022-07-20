using RootMotion.FinalIK.FitPlayProcedural;
using UnityEngine;

namespace VRIK.State
{
    public class LocomotionStateMachine<T> : ILocomotionStateMachine<T> where T : class
    {
        public LocomotionState<T> CurrentLocomotionState
        {
            get;
            private set;
        }

        public LocomotionState<T> PreLocomotionState
        {
            get;
            private set;
        }
        
        public LocomotionStateMachine(LocomotionState<T> initialLocomotionState)
        {
            Initialize(initialLocomotionState);
        }

        public void StartMachine()
        {
            CurrentLocomotionState.Enter();
        }
        
        public void OnUpdate(float deltaTime)
        {
            ProceduralState state = CurrentLocomotionState as ProceduralState;
            
            Debug.Log(state.stateEnum + " " + CurrentLocomotionState.GetType().FullName);
            
            
            CurrentLocomotionState?.Update(deltaTime);
            
            LocomotionState<T> nextState = CurrentLocomotionState?.Event();
            if (nextState == CurrentLocomotionState) return;
            CurrentLocomotionState?.Exit();
            PreLocomotionState = CurrentLocomotionState;
            CurrentLocomotionState = nextState;
            CurrentLocomotionState?.Enter();
        }

        public void ChangeState(ILocomotionState<T> nextState)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeState(LocomotionState<T> nextState)
        {
            if (nextState == CurrentLocomotionState) 
                return;
            
            CurrentLocomotionState?.Exit();
            PreLocomotionState = CurrentLocomotionState;
            CurrentLocomotionState = nextState;
            CurrentLocomotionState?.Enter();
        }

        private void Initialize(LocomotionState<T> initialLocomotionState)
        {
            if (initialLocomotionState == null)
            {
                Debug.LogError($"状态不存在{typeof(T)}");
                return;
            }

            PreLocomotionState = null;
            CurrentLocomotionState = initialLocomotionState;
           
        }
    }
}