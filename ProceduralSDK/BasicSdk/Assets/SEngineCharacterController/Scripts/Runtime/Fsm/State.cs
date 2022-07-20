using UnityEngine;

namespace SEngineCharacterController
{
    public class State<T> : IState<T> where T : class
    {
        /// <summary>
        /// 持有者
        /// </summary>
        public T Owner { get; private set; }
        

        public State(T owner)
        {
            Owner = owner;
        }

        public bool IsTermination { get; set; }

        public virtual void Enter()
        {
#if UNITY_EDITOR
            Debug.Log($"Enter state: {this}");
#endif
        }
        
        public virtual void Exit()
        {
#if UNITY_EDITOR
            Debug.Log($"Exit state: {this}");
#endif
        }
        
        public virtual void Tick(float deltaTime)
        {
            
        }

        public virtual IState<T> Event()
        {
            return this;
        }
    }
}