using System.Collections.Generic;
using Core.Services.Command;
using Core.Services.Logging;
using Core.States.Battle;
using Core.States.Battle.SubStates;
using Models;

namespace Commands.Battle
{
    public class PlayerTapCommand : ICommandItem
    {
        public CommandState CmdState { get; set; }
        public List<ICommandItem> ChildCommands { get; set; }

        private readonly string _selectedHero;
        private readonly bool _isPlayerHero;
        private IBattleState _battleState;
        private bool _attacked;

        public PlayerTapCommand(string selectedHero, bool isPlayerHero)
        {
            _selectedHero = selectedHero;
            _isPlayerHero = isPlayerHero;
        }


        public bool Validate(IBattleState battleState)
        {
            _battleState = battleState;

            if (_battleState.CurrentState is not PlayingState)
            {
                return false;
            }

            RuntimeGameModel gameState = _battleState.GameController.GetRuntimeState();
            if (_isPlayerHero)
            {
                if (!gameState.IsPlayerTurn())
                {
                    // not your turn
                    return false;
                }

                if (gameState.GetPlayerHeroModel(_selectedHero).IsDied())
                {
                    return false;
                }
            }

            // opponent's hero
            if (!_isPlayerHero)
            {
                if (gameState.IsPlayerTurn())
                {
                    // not enemy's turn
                    return false;
                }
                if (gameState.GetOpponentHeroModel(_selectedHero).IsDied())
                {
                    return false;
                }
            }

            if (gameState.IsTurnOver() || gameState.IsGameOver())
            {
                // this should never happen, we run state machine before commands !
                Log.Warning("game or turn must be over, something went wrong");
                return false;
            }
            return true;
        }

        public void Update(float deltaTime)
        {
            if (_attacked) return;
            _attacked = true;

            RuntimeGameModel gameState = _battleState.GameController.GetRuntimeState();

            HeroModel target = gameState.IsPlayerTurn() ?
                gameState.FindRandomOpponentHero() :
                gameState.FindRandomPlayerHero();

            HeroModel attacker = gameState.IsPlayerTurn()
                ? gameState.GetPlayerHeroModel(_selectedHero)
                : gameState.GetOpponentHeroModel(_selectedHero);

            attacker.Attack(target);

            // consume player move
            gameState.SetPlayerMoveSuccess();

            _battleState.AsyncService.BootstrapCoroutine(
                _battleState.AttackToTarget(target.Id, attacker.Id, gameState.IsPlayerTurn(), 10),
                OnAttackAnimCompleted);
        }

        void OnAttackAnimCompleted()
        {
            // add check end turn for next command
            ChildCommands = new List<ICommandItem>()
            {
                new EndTurnCommand()
            };
            CmdState = CommandState.Completed;
        }
    }
}