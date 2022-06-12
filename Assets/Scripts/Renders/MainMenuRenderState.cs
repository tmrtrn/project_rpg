using System;
using Core;
using Core.Events;
using Core.Services.Event;
using Data.Hero;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Renders
{
    /// <summary>
    /// Manages menu rendering
    /// </summary>
    public class MainMenuRenderState : MonoBehaviour, IGameRenderState
    {
        private IEventDispatcher _eventService;
        private RuntimeGameModel _runtimeGameState;

        [SerializeField]
        private Transform heroVPanelViewObject;
        [SerializeField]
        private GameObject heroPrefabObject;
        [SerializeField]
        private BattleButtonView battleButton;

        [SerializeField] private InfoPopUpView popupView;

        public void Inject(IEventDispatcher eventService, RuntimeGameModel runtimeGameState)
        {
            _eventService = eventService;
            _runtimeGameState = runtimeGameState;
        }

        public void Enter()
        {
            foreach (HeroModel heroModel in _runtimeGameState.HeroCollection)
            {
                CreateHeroCard(heroModel.GetHeroAsset());
            }
            popupView.Exit();
            UpdateBattleButtonState();
        }

        public void Exit()
        {
            popupView.Exit();
        }

        void CreateHeroCard(IHeroAsset asset)
        {
            GameObject heroViewObj = Instantiate(heroPrefabObject, heroVPanelViewObject);
            HeroCardView card = heroViewObj.GetComponent<HeroCardView>();
            card.Render(asset, _eventService, _runtimeGameState.IsHeroInPlayerTeam(asset.Id));
        }


        public void RenderCardInfoPopup(string heroId)
        {
            popupView.Render(_runtimeGameState.GetHeroModelAttribute(heroId));
        }

        public void UpdateBattleButtonState()
        {
            battleButton.SetReady(_runtimeGameState.IsReadyToFight());
        }
    }
}