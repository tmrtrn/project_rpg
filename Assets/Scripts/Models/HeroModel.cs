using System;
using Constants;
using Data.Hero;

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

        public float AttackByLevel => _heroAsset.Attributes.Attack + GetAttackBuffByLevel();
        public int Experience => _savedHero.experience;

        public float GetCurrentHp()
        {
            return _savedHero.battleHp;
        }

        public bool IsDied()
        {
            return _savedHero.battleHp <= 0;
        }

        public void ResetHpForBattle()
        {
            _savedHero.battleHp = FullHealth;
        }

        public float Attack(HeroModel target)
        {
            float damage = AttackByLevel;
            target.Damage(damage);
            return damage;
        }


        private void Damage(float damage)
        {
            _savedHero.battleHp = Math.Max(0, _savedHero.battleHp - damage);
        }
    }
}