using System;
using Data.Hero;
using DefaultNamespace;

namespace Models
{
    public class HeroModel : IHeroModelAttribute
    {
        private readonly IHeroAsset _heroAsset;
        private readonly SavedHeroModel _savedHero;

        public HeroModel(IHeroAsset heroAsset, SavedHeroModel savedHero)
        {
            _heroAsset = heroAsset;
            _savedHero = savedHero;
        }

        public IHeroAsset GetHeroAsset()
        {
            return _heroAsset;
        }

        public string Id => _heroAsset.Id;
        public string Name => _heroAsset.Attributes.Name;
        public int Level => Math.Max(1, _savedHero.level);

        private float GetHpBuffByLevel()
        {
            return _heroAsset.Attributes.Health * ((Level - 1) * GameConstants.IncreaseHealthPerLevel);
        }

        public float FullHealth => _heroAsset.Attributes.Health + GetHpBuffByLevel();

        private float GetAttackBuffByLevel()
        {
            return _heroAsset.Attributes.Attack * ((Level - 1) * GameConstants.IncreaseAttackPerLevel);
        }

        public float FullAttack => _heroAsset.Attributes.Attack + GetAttackBuffByLevel();

        public int Experience => _savedHero.experience;
    }
}