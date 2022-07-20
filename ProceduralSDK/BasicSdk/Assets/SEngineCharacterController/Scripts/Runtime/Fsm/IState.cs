namespace SEngineCharacterController
{
    /// <summary>
    /// 状态机 状态接口
    /// </summary>
    public interface IState<T> where T : class
    {
        /// <summary>
        /// 是否终止
        /// </summary>
        bool IsTermination { set; get; }
        
        /// <summary>
        /// 进入状态
        /// </summary>
        void Enter();

        /// <summary>
        /// 退出状态
        /// </summary>
        void Exit();
        
        void Tick(float deltaTime);
        
        IState<T> Event();
    }
}