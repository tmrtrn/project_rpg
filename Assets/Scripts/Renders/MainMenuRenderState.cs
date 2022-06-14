using System;
using Core;
using Core.Events;
using Core.Services.Event;
using Core.Services.Logging;
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
            battleButton.InjectServices(eventService);
        }
        public void Enter()
        {
            foreach (HeroModel heroModel in _runtimeGameState.PlayerHeroCollection)
            {
                CreateHeroCard(heroModel.GetHeroAsset());
            }
            popupView.Exit();
            UpdateBattleButtonState();
            battleButton.OnClicked += OnBattleButtonClicked;
        }


        public void Exit()
        {
            popupView.Exit();
            battleButton.OnClicked -= OnBattleButtonClicked;
        }

        void CreateHeroCard(IHeroAsset asset)
        {
            GameObject heroViewObj = Instantiate(heroPrefabObject, heroVPanelViewObject);
            HeroCardView card = heroViewObj.GetComponent<HeroCardView>();
            card.Render(asset, _eventService, _runtimeGameState.IsHeroInPlayerTeam(asset.Id));
        }


        public void RenderCardInfoPopup(string heroId)
        {
            popupView.Render(_runtimeGameState.GetPlayerHeroModel(heroId), false);
        }

        public void UpdateBattleButtonState()
        {
            battleButton.SetReady(_runtimeGameState.IsReadyToFight());
        }

        private void OnBattleButtonClicked()
        {
            // check this again
            if (_runtimeGameState.IsReadyToFight())
            {
                _eventService.Publish(new MenuCompletedEvent());
            }
            else
            {
                Log.Error("Battle button can not be interactible until the team is ready");
            }
        }
    }
}