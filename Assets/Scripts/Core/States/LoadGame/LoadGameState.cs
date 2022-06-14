using System;
using Constants;
using Core.Events;
using Core.Services.Data;
using Core.Services.Event;
using Core.Services.Logging;
using Core.Services.Scene;
using Data.Hero;

namespace Core.States.LoadGame
{
    /// <summary>
    /// State that loads necessary data before progressing to play.
    /// </summary>
    public class LoadGameState : BaseGameState
    {
        private readonly IGameController _gameController;
        private readonly ISceneService _sceneService;
        private readonly IEventDispatcher _eventDispatcher;

        public LoadGameState(IGameController gameController, ISceneService sceneService, IEventDispatcher eventDispatcher)
        {
            _gameController = gameController;
            _sceneService = sceneService;
            _eventDispatcher = eventDispatcher;
        }

        public override void Enter(object context)
        {
            base.Enter(context);

            _eventDispatcher.SubscribeOnce<PreLoadAssetsEvent>(_ =>
            {
                Log.Info("Hero assets received");
                _gameController.GenerateRuntimeData();
                string sceneNameToLoad = _gameController.HasActiveBattle()
                    ? GameConstants.SceneNameBattle
                    : GameConstants.SceneNameMenu;
                _eventDispatcher.Publish(new ChangeSceneEvent(sceneNameToLoad));
            });
            _gameController.PreloadAssets();
        }
    }
}