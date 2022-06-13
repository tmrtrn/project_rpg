using System;
using Core.Events;
using Core.Services.Async;
using Core.Services.Command;
using Core.Services.Event;
using Core.Services.Logging;
using Core.Services.State;
using Core.States.Battle.SubStates;
using Renders;
using Object = UnityEngine.Object;

namespace Core.States.Battle
{
    public class BattleState : BaseGameState, IBattleState
    {

        /// <summary>
        /// Controls battle states.
        /// </summary>
        private FiniteStateMachine _fsm;

        private readonly GameController _gameController;
        private readonly IEventDispatcher _eventService;
        private readonly IAsyncService _asyncService;

        private readonly ICommandWorker _commandWorker;
        private BattleView _battleView;

        public BattleState(
            GameController gameController,
            IEventDispatcher eventService,
            IAsyncService asyncService)
        {
            _gameController = gameController;
            _eventService = eventService;
            _asyncService = asyncService;
            // create command worker for game actions, like tap, enemy action
            _commandWorker = new CommandWorker(this);
        }


        public override void Enter(object context)
        {
            base.Enter(context);
            IState[] states = new IState[]
            {
                new PrePlayState(this),
                new PlayingState(this, _eventService),
                new GameResultState()
            };

            _fsm = new FiniteStateMachine(states);

            _battleView = Object.FindObjectOfType<BattleView>();
            _fsm.Change<PrePlayState>();
        }

        public override void UpdateState(float deltaTime)
        {
            base.UpdateState(deltaTime);
            // update child state
            _fsm.Update(deltaTime);
            _commandWorker.RunUpdate(deltaTime);
        }

        public bool AddCommand(ICommandItem item)
        {
            // command queue is in process
            if (!_commandWorker.ClearProceed()) return false;
            _commandWorker.AddCommand(item);
            return true;
        }

        public override void Exit()
        {
            base.Exit();
            _fsm.Current.Exit();
            _battleView.Exit();
            _commandWorker.Cancel();
        }

        public GameController GameController => _gameController;
        public IEventDispatcher EventService => _eventService;
        public IAsyncService AsyncService => _asyncService;

        public void ChangeState<T>(object context = null) where T : IState
        {
            _fsm.Change<T>(context);
        }

        public IState CurrentState => _fsm.Current;

        public BattleView BattleView => _battleView;

    }
}