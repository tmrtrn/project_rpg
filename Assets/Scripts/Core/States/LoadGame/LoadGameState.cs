using System;
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
        private readonly GameController _gameController;
        private readonly ISceneService _sceneService;
        private readonly IEventDispatcher _eventDispatcher;

        public LoadGameState(GameController gameController, ISceneService sceneService, IEventDispatcher eventDispatcher)
        {
            _gameController = gameController;
            _sceneService = sceneService;
            _eventDispatcher = eventDispatcher;
        }

        public override void Enter(object context)
        {
            base.Enter(context);

            var waits = new Action<Action>[]
            {
                WaitForAssets,
                WaitForSceneLoad
            };

            // waiting for all process to complete
            var calls = 0;
            Action callback = () =>
            {
                if (++calls == waits.Length)
                {
                    Log.Info( "all pre requires received. ");
                    _eventDispatcher.Publish(new LoadingCompletedEvent());
                }
                else
                {
                    Log.Info( "Still waiting on {0}.", waits.Length - calls);
                }
            };

            for (int i = 0, len = waits.Length; i < len; i++)
            {
                // subscribe to events
                waits[i](callback);
            }

            _sceneService.LoadScene("Main");
            _gameController.PreloadAssets();
            _gameController.GenerateRuntimeData();
        }

        private void WaitForAssets(Action callback)
        {
            Log.Info("Waiting for assets");
            _eventDispatcher.SubscribeOnce<PreLoadAssetsEvent>(_ =>
            {
                Log.Info("Hero assets received");
                callback();
            });
        }

        private void WaitForSceneLoad(Action callback)
        {
            Log.Info("Waiting for scene loading");
            _eventDispatcher.SubscribeOnce<SceneLoaded>(_ =>
            {
                Log.Info("Scene is loaded");
                callback();
            });
        }
    }
}