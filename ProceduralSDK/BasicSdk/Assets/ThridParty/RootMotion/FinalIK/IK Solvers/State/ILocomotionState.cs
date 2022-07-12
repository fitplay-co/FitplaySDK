namespace VRIK.State
{
    public interface ILocomotionState<T> where T : class
    {
        void Enter();

        void Exit();

        void Update(float deltaTime);

        LocomotionState<T> Event();
    }
}