using UnityEngine;

namespace VRIK.State
{
    public class LocomotionState<T> : ILocomotionState<T> where T : class
    {
        public T Owner { get; private set; }
        public LocomotionState<T> NextState;

        public LocomotionState(T owner)
        {
            Owner = owner;
        }

        public virtual void Enter()
        {
#if UNITY_EDITOR
            //Debug.Log($"Enter state: {this}");
#endif
        }
        
        public virtual void Exit()
        {
#if UNITY_EDITOR
            //Debug.Log($"Exit state: {this}");
#endif
        }
        
        public virtual void Update(float deltaTime)
        {
            
        }

        public virtual LocomotionState<T> Event()
        {
            return this;
        }

    }
}