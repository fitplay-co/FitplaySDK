namespace MotionCaptureBasic.FSM
{
    public interface IStateMachine<TOwner> where TOwner : class
    {
        void ChangeState(State<TOwner> state);

        void ChangePrevState();

        void OnTick(float deltaTime);
    }
}