using Core.Events;
using Core.Services.Event;
using Core.Services.State;

namespace Core.States.Initialize
{
    /// <summary>
    /// This state should works only once
    /// just ignore the state and move to the loading state
    /// </summary>
    public class InitializeGameState : IState
    {
        private readonly IEventDispatcher _eventService;

        public InitializeGameState(IEventDispatcher eventService)
        {
            _eventService = eventService;
        }

        public void Enter(object context)
        {
            _eventService.Publish(new GameInitializedEvent());
        }

        public void UpdateState(float deltaTime)
        {

        }

        public void Exit()
        {

        }
    }
}