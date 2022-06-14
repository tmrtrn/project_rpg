using Core.Services.State;

namespace Core.States
{
    /// <summary>
    /// To exit from the current state before destroying the active scene
    /// </summary>
    public class EmptyState : IState
    {
        public void Enter(object context)
        {

        }

        public void UpdateState(float deltaTime)
        {

        }

        public void Exit()
        {

        }
    }
}