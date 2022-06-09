namespace Core.Services.State
{
    public interface IState
    {
        void Enter(object context);
        void UpdateState(float deltaTime);
        void Exit();
    }
}