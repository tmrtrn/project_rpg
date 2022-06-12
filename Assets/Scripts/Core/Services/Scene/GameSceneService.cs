using System;
using System.Collections;
using Core.Events;
using Core.Services.Async;
using Core.Services.Event;
using Core.Services.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.Scene
{
    /// <summary>
    /// Responsible to load unity scenes
    /// </summary>
    public class GameSceneService : ISceneService
    {
        private readonly IAsyncService _asyncService;
        private readonly IEventDispatcher _eventService;

        public GameSceneService(
            IAsyncService asyncService,
            IEventDispatcher eventDispatcher)
        {
            _asyncService = asyncService;
            _eventService = eventDispatcher;
        }

        public void LoadScene(string scene)
        {
            try
            {
                _asyncService.BootstrapCoroutine(LoadSceneAsync(scene), () =>
                {
                    _eventService.Publish(new SceneLoaded(scene));
                });
            }
            catch (Exception ex)
            {
                Log.Error($"An exception is occured while loading scene {scene}", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Loads a scene with async way
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(string scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
            // Wait until the asynchronous scene fully loaded
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}