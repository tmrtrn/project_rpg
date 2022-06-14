using System;
using Constants;
using Core;
using Core.Events;
using Core.Events.GameEvents;
using Core.Services.Event;
using Data.Hero;
using TMPro;
using UnityEngine;

namespace Renders
{
    public class GameResultView : MonoBehaviour, IBattleRenderer
    {
        [SerializeField] private GameObject parentCanvas;
        [SerializeField] private TMP_Text title;

        [Header("Hero Result panels")]
        [SerializeField] private GameResultSectionPanel experiencePanel;
        [SerializeField] private GameResultSectionPanel levelUpPanel;
        [SerializeField] private GameResultSectionPanel unlockedHeroPanel;

        private IGameController _gameController;
        private IEventDispatcher _eventService;
        private Action _unSubEventResult;

        public void InjectServices(IGameController gameController, IEventDispatcher eventService)
        {
            _gameController = gameController;
            _eventService = eventService;
            experiencePanel.Inject(_eventService);
            unlockedHeroPanel.Inject(_eventService);
            levelUpPanel.Inject(_eventService);
        }

        public void Enter(object context)
        {
            _unSubEventResult = _eventService.Subscribe<GameResultEvent>(OnGameResultReceived);
            parentCanvas.SetActive(true);
        }

        private void OnGameResultReceived(GameResultEvent result)
        {
            title.text = result.isPlayerWon ? "YOU WON!" : "YOU LOST";

            experiencePanel.SetVisible(result.gainExperienceHeroList != null && result.gainExperienceHeroList.Count > 0);
            levelUpPanel.SetVisible(result.levelUpHeroList != null && result.levelUpHeroList.Count > 0);
            unlockedHeroPanel.SetVisible(!string.IsNullOrEmpty(result.UnlockedHeroId));

            if (result.gainExperienceHeroList != null)
            {
                foreach (string heroId in result.gainExperienceHeroList)
                {
                    _gameController.GetHeroAssetById(heroId, out IHeroAsset hero);
                    experiencePanel.CreateCard(hero);
                }
            }

            if (result.levelUpHeroList != null)
            {
                foreach (string heroId in result.levelUpHeroList)
                {
                    _gameController.GetHeroAssetById(heroId, out IHeroAsset hero);
                    levelUpPanel.CreateCard(hero);
                }
            }

            if (!string.IsNullOrEmpty(result.UnlockedHeroId))
            {
                _gameController.GetHeroAssetById(result.UnlockedHeroId, out IHeroAsset hero);
                unlockedHeroPanel.CreateCard(hero);
            }
        }

        public void UpdateState(float deltaTime)
        {

        }

        public void OnReturnMenuButtonClicked()
        {
            _eventService.Publish(new ChangeSceneEvent(GameConstants.SceneNameMenu));
        }

        public void Exit()
        {
            parentCanvas.SetActive(false);
            _unSubEventResult();
        }
    }
}