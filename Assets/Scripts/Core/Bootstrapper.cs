using System;
using Core.Services;
using Core.Services.Async;
using Core.Services.Data;
using Core.Services.Event;
using Core.Services.Logging;
using Core.Services.Scene;
using Core.Services.State;
using Core.States;
using Core.States.Initialize;
using Core.States.LoadGame;
using Core.States.MainGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Core
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            // targets Unity logging
            Log.SetLogTarget(new UnityLogTarget());


            // Create Services
            EventDispatcher eventService = new EventDispatcher();
            IAsyncService asyncService = CreateMonoService<UnityAsyncService>();
            GameSceneService sceneService = new GameSceneService(asyncService, eventService);
            GameDataService dataService = new GameDataService();
            GameController gameController = new GameController(dataService, eventService);


            // Create States
            InitializeGameState initializeGameState = new InitializeGameState(eventService);
            LoadGameState loadGameState = new LoadGameState(gameController, sceneService, eventService);
            MenuState menuState = new MenuState(eventService, gameController);

            IState[] states = new IState[]
            {
                initializeGameState,
                loadGameState,
                menuState
            };

            // Game State service controls major states
            GameStateService gameStateService = new GameStateService(eventService, states);
            Object.FindObjectOfType<ServiceManager>().RegisterGameStateService(gameStateService);
        }

        private static T CreateMonoService<T>() where T: IService
        {
            GameObject serviceContainerObject = Object.Instantiate(new GameObject());
            serviceContainerObject.name = nameof(T);
            Type contractType = typeof(T);
            serviceContainerObject.AddComponent(contractType);
            Object.DontDestroyOnLoad(serviceContainerObject);
            return serviceContainerObject.GetComponent<T>();
        }

    }
}