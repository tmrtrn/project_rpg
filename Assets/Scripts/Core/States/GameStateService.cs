using System;
using Constants;
using Core.Events;
using Core.Services;
using Core.Services.Event;
using Core.Services.Logging;
using Core.Services.Scene;
using Core.Services.State;
using Core.States.Battle;
using Core.States.Initialize;
using Core.States.LoadGame;
using Core.States.MainGame;
using UnityEngine;

namespace Core.States
{
    /// <summary>
    /// Manages major states of the game.
    /// </summary>
    public class GameStateService : IGameStateService
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISceneService _sceneService;
        private readonly IGameController _gameController;
        /// <summary>
        /// Controls game states.
        /// </summary>
        private readonly FiniteStateMachine _fsm;

        private Action _unsubInit;
        private Action _unSubMenuCompleted;
        private Action _unsubChangeScene;

        public GameStateService(
            IEventDispatcher eventDispatcher,
            ISceneService sceneService,
            IGameController gameController,
            IState[] states)
        {
            _eventDispatcher = eventDispatcher;
            _sceneService = sceneService;
            _gameController = gameController;
            _fsm = new FiniteStateMachine(states);
        }

        public void Start()
        {
            _unsubInit = _eventDispatcher.Subscribe<GameInitializedEvent>(GameInitialized);
            _unSubMenuCompleted = _eventDispatcher.Subscribe<MenuCompletedEvent>(OnMenuCompletedEvent);
            _unsubChangeScene = _eventDispatcher.Subscribe<ChangeSceneEvent>(ChangeSceneReceived);
            _fsm.Change<InitializeGameState>();
        }

        /// <summary>
        ///  Called when game initialized message is published.
        /// </summary>
        /// <param name="obj"></param>
        private void GameInitialized(GameInitializedEvent obj)
        {
            Log.Info("Game initialized.");
            _fsm.Change<LoadGameState>();
        }


        /// <summary>
        /// Called when battle button is pressed
        /// </summary>
        /// <param name="event"></param>
        void OnMenuCompletedEvent(MenuCompletedEvent @event)
        {
            Log.Info("Ready to load battle");
            _fsm.Change<LoadBattleState>();
        }

        /// <summary>
        /// Triggers when scene loading is requested
        /// </summary>
        /// <param name="request"></param>
        private void ChangeSceneReceived(ChangeSceneEvent request)
        {
            Log.Info("Change scene is received");
            // to exit active one, our exit methods may use the unity gameobjects
            _fsm.Change<EmptyState>();

            _eventDispatcher.SubscribeOnce<SceneLoaded>((loaded) =>
            {
                switch (loaded.SceneName)
                {
                    case GameConstants.SceneNameMenu:
                        _fsm.Change<MenuState>();
                        break;
                    case GameConstants.SceneNameBattle:
                        _fsm.Change<BattleState>();
                        break;
                    default:
                        Log.Error($"undefined scene name: {loaded.SceneName}");
                        break;
                }
            });
            _sceneService.LoadScene(request.sceneName);
        }

        public void Stop()
        {
            _unsubInit();
            _unSubMenuCompleted();
            _unsubChangeScene();
        }

        public void Update()
        {
            _fsm.Update(Time.deltaTime);
        }

        // save player progress
        public void ApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _gameController.SavePlayerProgress();
            }
        }

        public void ApplicationQuit()
        {
            _gameController.SavePlayerProgress();
        }
    }
}