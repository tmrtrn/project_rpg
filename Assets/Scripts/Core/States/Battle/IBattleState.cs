using Core.Services.Async;
using Core.Services.Command;
using Core.Services.Event;
using Core.Services.State;
using Renders;

namespace Core.States.Battle
{
    public interface IBattleState
    {
        GameController GameController { get; }
        IEventDispatcher EventService { get; }
        IAsyncService AsyncService { get; }
        void ChangeState<T>(object context = null) where T : IState;
        IState CurrentState { get; }
        BattleView BattleView { get; }
        bool AddCommand(ICommandItem item);
    }
}