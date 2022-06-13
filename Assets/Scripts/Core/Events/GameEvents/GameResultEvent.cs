using System.Collections.Generic;
using Core.Services.Event;

namespace Core.Events.GameEvents
{
    public class GameResultEvent : IGameEvent
    {
        public readonly bool isPlayerWon;
        public readonly List<string> gainExperienceHeroList;
        public readonly List<string> levelUpHeroList;
        public readonly string rewardedHero;

        public GameResultEvent(
            bool isPlayerWon,
            List<string> gainExperienceHeroList,
            List<string> levelUpHeroList,
            string rewardedHero)
        {
            this.isPlayerWon = isPlayerWon;
            this.gainExperienceHeroList = gainExperienceHeroList;
            this.levelUpHeroList = levelUpHeroList;
            this.rewardedHero = rewardedHero;
        }
    }
}