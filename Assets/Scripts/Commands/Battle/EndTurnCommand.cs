using System.Collections;
using System.Collections.Generic;
using Core.Events.GameEvents;
using Core.Services.Command;
using Core.States.Battle;
using Core.States.Battle.SubStates;
using Models;
using UnityEngine;

namespace Commands.Battle
{
    public class EndTurnCommand : ICommandItem
    {
        private IBattleState _battleState;
        private bool _changed = false;

        public bool Validate(IBattleState battleState)
        {
            _battleState = battleState;

            if (_battleState.CurrentState is not PlayingState)
            {
                return false;
            }
            RuntimeGameModel gameState = _battleState.GameController.GetRuntimeState();
            if (!gameState.IsTurnOver())
            {
                return false;
            }

            return true;
        }

        public void Update(float deltaTime)
        {
            if (_changed) return;
            _changed = true;
            RuntimeGameModel gameState = _battleState.GameController.GetRuntimeState();
            gameState.EndTurn();

            // check the game is over
            if (gameState.IsGameOver())
            {
                _battleState.AsyncService.BootstrapCoroutine(MoveToResultGame());
                return;
            }

            // play for AI
            ChildCommands = new List<ICommandItem>()
            {
                new PlayerTapCommand(gameState.FindRandomOpponentHero().Id, false)
            };
            // TODO: Call UI end turn animations

            // update whois turn over screen
            _battleState.EventService.Publish(new PlayingStateChangedEvent(gameState.IsPlayerTurn(), false));

            CmdState = CommandState.Completed;
        }

        public CommandState CmdState { get; set; }
        public List<ICommandItem> ChildCommands { get; set; }

        private IEnumerator MoveToResultGame()
        {
            _battleState.EventService.Publish(new PlayingStateChangedEvent(false, true));
            // fake wait processing
            yield return new WaitForSeconds(1.5f);
            CmdState = CommandState.Completed;
            _battleState.ChangeState<GameResultState>();
        }
    }
}