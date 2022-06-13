using System;
using Commands.Battle;
using Core.Events;
using Core.Events.GameEvents;
using Core.Services.Event;
using Models;
using Renders;

namespace Core.States.Battle.SubStates
{
    public class PlayingState : BaseGameState
    {
        private readonly IBattleState _battleState;
        private readonly RuntimeGameModel _gameState;
        private readonly IEventDispatcher _eventService;

        private Action _unsubCardInput;

        public PlayingState(IBattleState battleState)
        {
            _battleState = battleState;
            _eventService = _battleState.EventService;
            _gameState = _battleState.GameController.GetRuntimeState();
        }

        public override void Enter(object context)
        {
            base.Enter(context);
            _battleState.ChangeRenderState<BattleView>();
            _eventService.Publish(new PlayingStateChangedEvent(_gameState.IsPlayerTurn(), false));
            _unsubCardInput = _eventService.Subscribe<HeroCardInputEvent>(OnHeroCardInputEvent);
        }

        public override void Exit()
        {
            base.Exit();
            _unsubCardInput();
            _battleState.ClearCommands();
        }

        void OnHeroCardInputEvent(HeroCardInputEvent inputEvent)
        {
            if (inputEvent.inputType == HeroCardInputEvent.InputType.Tap)
            {
                string heroId = inputEvent.card.GetHeroId();
                _battleState.AddCommand(new PlayerTapCommand(heroId, true));
            }
        }
    }
}