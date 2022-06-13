using System;
using Core;
using Core.Events.GameEvents;
using Core.Services.Event;
using TMPro;
using UnityEngine;

namespace Renders
{
    public class GameResultView : MonoBehaviour, IBattleRenderer
    {
        [SerializeField] private TMP_Text _title;

        private GameController _gameController;
        private IEventDispatcher _eventService;
        private Action _unSubEventResult;

        public void InjectServices(GameController gameController, IEventDispatcher eventService)
        {
            _gameController = gameController;
            _eventService = eventService;
        }

        public void Enter(object context)
        {
            _unSubEventResult = _eventService.Subscribe<GameResultEvent>(OnGameResultReceived);
        }

        private void OnGameResultReceived(GameResultEvent obj)
        {

        }

        public void UpdateState(float deltaTime)
        {

        }

        public void Exit()
        {
            _unSubEventResult();
        }
    }
}