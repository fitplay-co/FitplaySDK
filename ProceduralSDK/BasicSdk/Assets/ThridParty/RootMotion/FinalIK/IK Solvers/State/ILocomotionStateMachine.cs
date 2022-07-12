namespace VRIK.State
{
    public interface ILocomotionStateMachine<T> where T : class
    {
        void OnUpdate(float deltaTime);
        void ChangeState(LocomotionState<T> nextState);
    }
}