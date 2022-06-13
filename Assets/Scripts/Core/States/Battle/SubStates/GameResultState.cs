using System.Collections.Generic;
using Constants;
using Core.Events.GameEvents;
using Core.Services.Logging;
using Models;
using Renders;

namespace Core.States.Battle.SubStates
{
    public class GameResultState : BaseGameState
    {
        private readonly IBattleState _battleState;

        public GameResultState(IBattleState battleState)
        {
            _battleState = battleState;
        }

        public override void Enter(object context)
        {
            Log.Info("Game is over");

            _battleState.ChangeRenderState<GameResultView>();

            base.Enter(context);

            // set battle is over
            RuntimeGameModel gameState = _battleState.GameController.GetRuntimeState();
            gameState.SetBattleOver();

            bool isPlayerWon = false;

            // find experienced and leveled up heroes
            List<string> gainExperienceHeroList = new List<string>();
            List<string> levelUpHeroList = new List<string>();
            string rewardedHero = null;

            if (!gameState.IsAnyAliveOpponentMember())
            {
                // you win
                isPlayerWon = true;
                // increase alive hero's experience of player
                foreach (HeroModel alivePlayerHero in gameState.GetAlivePlayerHeroes())
                {
                    gainExperienceHeroList.Add(alivePlayerHero.Id);
                    bool levelUp = alivePlayerHero.IncreaseExperience(1, GameConstants.NextLevelExperienceThreshold);
                    if (levelUp)
                    {
                        levelUpHeroList.Add(alivePlayerHero.Id);
                    }
                }
            }

            if (gameState.GetPlayedBattleCount() % GameConstants.PlayedGameCountForHeroReward == 0
                && gameState.PlayerHeroCollection.Count < GameConstants.PlayerCollectionHeroLimit)
            {
                if (gameState.GenerateAndAddNewHeroModel(out HeroModel unlockedHero))
                {
                    rewardedHero = unlockedHero.Id;
                }
            }

            GameResultEvent resultEvent = new GameResultEvent(
                isPlayerWon,
                gainExperienceHeroList,
                levelUpHeroList,
                rewardedHero);
            _battleState.EventService.Publish(resultEvent);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}