using System.Collections;
using Core.Services.Async;
using Core.Services.Command;
using Core.Services.Event;
using Core.Services.State;
using Renders;

namespace Core.States.Battle
{
    public interface IBattleState
    {
        IGameController GameController { get; }
        IEventDispatcher EventService { get; }
        IAsyncService AsyncService { get; }
        void ChangeState<T>(object context = null) where T : IState;
        IState CurrentState { get; }
        IEnumerator AttackToTarget(string targetId, string attacker, bool playerAttack, float damage);
        bool AddCommand(ICommandItem item, bool checkClear = true);
        void ClearCommands();
        void ChangeRenderState<T>() where T : IBattleRenderer;
    }
}