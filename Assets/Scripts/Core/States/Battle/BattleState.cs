using System;
using System.Collections;
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

        private readonly IGameController _gameController;
        private readonly IEventDispatcher _eventService;
        private readonly IAsyncService _asyncService;

        private readonly ICommandWorker _commandWorker;

        private FiniteStateMachine _fsmRenderer;
        private IBattleRenderer[] _battleRenderers;

        public BattleState(
            IGameController gameController,
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
                new PlayingState(this),
                new GameResultState(this)
            };

            _fsm = new FiniteStateMachine(states);

            _battleRenderers = new IBattleRenderer[]
            {
                new InitialBattleView(),
                Object.FindObjectOfType<BattleView>(),
                Object.FindObjectOfType<GameResultView>()
            };

            foreach (IBattleRenderer renderer in _battleRenderers)
            {
                renderer.InjectServices(_gameController, _eventService);
            }

            _fsmRenderer = new FiniteStateMachine(_battleRenderers);
            _fsmRenderer.Change<InitialBattleView>(); // initial with empty render state

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

        public void ClearCommands()
        {
            _commandWorker.Cancel();
        }

        public override void Exit()
        {
            base.Exit();
            _fsm.Current.Exit();
            _fsmRenderer.Current?.Exit();
            _commandWorker.Cancel();
        }

        public IGameController GameController => _gameController;
        public IEventDispatcher EventService => _eventService;
        public IAsyncService AsyncService => _asyncService;

        public void ChangeState<T>(object context = null) where T : IState
        {
            _fsm.Change<T>(context);
        }

        public IState CurrentState => _fsm.Current;

        public void ChangeRenderState<T>() where T : IBattleRenderer
        {
            _fsmRenderer.Change<T>();
        }

        public IEnumerator AttackToTarget(string targetId, string attacker, bool playerAttack, float damage)
        {
            if (_fsmRenderer.Current is BattleView battleView)
            {
                yield return battleView.AttackToTarget(targetId, attacker, playerAttack, damage);
                yield break;
            }
            Log.Error($"Can not attack in {_fsmRenderer.Current} render state ");
        }
    }
}