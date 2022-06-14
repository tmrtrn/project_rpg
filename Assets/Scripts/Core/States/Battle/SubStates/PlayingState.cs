using System;
using System.Collections;
using Commands.Battle;
using Core.Events;
using Core.Events.GameEvents;
using Core.Services.Async;
using Core.Services.Event;
using Models;
using Renders;
using UnityEngine;

namespace Core.States.Battle.SubStates
{
    public class PlayingState : BaseGameState
    {
        private readonly IBattleState _battleState;
        private readonly RuntimeGameModel _gameState;
        private readonly IEventDispatcher _eventService;
        private readonly IAsyncService _asyncService;

        private Action _unsubCardInput;

        public PlayingState(IBattleState battleState)
        {
            _battleState = battleState;
            _eventService = _battleState.EventService;
            _asyncService = _battleState.AsyncService;
            _gameState = _battleState.GameController.GetRuntimeState();
        }

        public override void Enter(object context)
        {
            base.Enter(context);
            _battleState.ChangeRenderState<BattleView>();
            _eventService.Publish(new PlayingStateChangedEvent(_gameState.IsPlayerTurn(), false));
            _unsubCardInput = _eventService.Subscribe<HeroCardInputEvent>(OnHeroCardInputEvent);

            // send enturn command for remain games
            var endTurnCommand = new EndTurnCommand();
            if (endTurnCommand.Validate(_battleState))
            {
                _battleState.AddCommand(endTurnCommand, false);
            }
            else if (!_gameState.IsPlayerTurn()) // ai's turn
            {
                // wait to see the attack
                _asyncService.Wait(1000, SendAiTapCommandWhenRemainGameIsStarted);
            }
        }

        void SendAiTapCommandWhenRemainGameIsStarted()
        {
            string randomOpponentHero = _battleState.GameController.GetRuntimeState().FindRandomOpponentHero().Id;
            _battleState.AddCommand(new PlayerTapCommand(randomOpponentHero, false), false);
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