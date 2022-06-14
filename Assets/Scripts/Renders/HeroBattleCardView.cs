using Core.Services.Event;
using DG.Tweening;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Renders
{
    public class HeroBattleCardView : HeroCardView, IHeroBattleCardView
    {

        [SerializeField] private Slider _healthBar;
        [SerializeField] private Transform _target;
        private HeroModel _heroModel;

        public void Render(HeroModel heroModel, IEventDispatcher eventService, bool opponentCard = false)
        {
            _heroModel = heroModel;
            base.isOpponentCard = opponentCard;
            base.Render(heroModel.GetHeroAsset(), eventService, false);
            SetHealth(heroModel.GetCurrentHp(), heroModel.FullHealth, false);
        }

        public Transform TargetPoint => _target;

        public HeroCardView GetView()
        {
            return this;
        }

        public void UpdateHealth()
        {
            SetHealth(_heroModel.GetCurrentHp(), _heroModel.FullHealth);
        }

        public void SetHealth(float health, float max, bool withAnim = true)
        {
            _healthBar.DOKill();
            float value = Mathf.Clamp(health / max, 0f, 1f);
            if (withAnim)
            {
                _healthBar.DOValue(value, 0.25f).SetEase(Ease.Linear);
            }
            else
            {
                _healthBar.value = value;
            }

            if (_heroModel.IsDied())
            {
                Die();
            }
        }

        private void Die()
        {
            Color color = GetAvatarImage().color;
            GetAvatarImage().color = new Color(color.r, color.g, color.b, 0.1f);
        }
    }
}