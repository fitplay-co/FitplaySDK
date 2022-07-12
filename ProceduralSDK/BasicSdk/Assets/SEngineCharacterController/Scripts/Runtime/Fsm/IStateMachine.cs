namespace SEngineCharacterController
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine<T> where T : class
    {
        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="nextState"></param>
        void ChangeState(IState<T> nextState);
        
        void OnTick(float deltaTime);
        
        /// <summary>
        /// 终止某个状态
        /// </summary>
        /// <param name="state"></param>
        void OnTermination(IState<T> state);
    }
}