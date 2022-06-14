using Core;
using Core.Services.Event;
using Core.Services.State;

namespace Renders
{
    public interface IBattleRenderer : IState
    {
        void InjectServices(IGameController gameController, IEventDispatcher eventService);
    }
}