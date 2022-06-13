using Core.Services.Event;
using Models;
using Renders;
using UnityEngine;

namespace Core.States.Battle.SubStates
{
    public class PrePlayState : BaseGameState
    {
        private BattleView _battleView;
        private IBattleState _battleState;

        public PrePlayState(IBattleState battleState)
        {
            _battleState = battleState;
        }

        public override void Enter(object context)
        {
            base.Enter(context);

            // create the battle model
            RuntimeGameModel runtimeGame = _battleState.GameController.GetRuntimeState();
            if (!runtimeGame.HasActiveBattle())
            {
                runtimeGame.CreateNewBattle();
            }

            _battleView = Object.FindObjectOfType<BattleView>();
            _battleView.InjectServices(_battleState.GameController, _battleState.EventService);

            _battleView.Enter();

            _battleState.ChangeState<PlayingState>();
        }
    }
}