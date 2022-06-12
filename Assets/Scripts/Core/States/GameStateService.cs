using System;
using Core.Events;
using Core.Services;
using Core.Services.Event;
using Core.Services.Logging;
using Core.Services.State;
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
        /// <summary>
        /// Controls game states.
        /// </summary>
        private readonly FiniteStateMachine _fsm;

        private Action _unsubInit;
        private Action _unSunLoading;

        public GameStateService(IEventDispatcher eventDispatcher, IState[] states)
        {
            _eventDispatcher = eventDispatcher;
            _fsm = new FiniteStateMachine(states);
        }

        public void Start()
        {
            _unsubInit = _eventDispatcher.Subscribe<GameInitializedEvent>(GameInitialized);
            _unSunLoading = _eventDispatcher.Subscribe<LoadingCompletedEvent>(GameLoadingCompleted);
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
        /// Called when the loading state is completed
        /// </summary>
        /// <param name="obj"></param>
        private void GameLoadingCompleted(LoadingCompletedEvent obj)
        {
            Log.Info("Loading completed");
            _fsm.Change<MenuState>();
        }

        public void Stop()
        {
            _unsubInit();
            _unSunLoading();
        }

        public void Update()
        {
            _fsm.Update(Time.deltaTime);
        }

    }
}