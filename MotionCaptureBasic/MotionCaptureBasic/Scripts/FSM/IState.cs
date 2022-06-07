namespace MotionCaptureBasic.FSM
{
    public interface IState
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        void Enter();

        /// <summary>
        /// 退出状态
        /// </summary>
        void Exit();

        void Tick(float deltaTime);
    }
}