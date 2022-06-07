namespace MotionCaptureBasic.FSM
{
    public abstract class State<TOwner> : IState where TOwner : class
    {
        /// <summary>
        /// 持有者
        /// </summary>
        public TOwner Owner { get; private set; }

        public State(TOwner owner)
        {
            Owner = owner;
        }

        public virtual void Enter()
        {
            
        }
        
        public virtual void Exit()
        {
            
        }
        
        public virtual void Tick(float deltaTime)
        {
            
        }
    }
}