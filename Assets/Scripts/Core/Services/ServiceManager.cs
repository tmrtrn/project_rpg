using System;
using Core.Services.Logging;
using UnityEngine;

namespace Core.Services
{
    public class ServiceManager : MonoBehaviour
    {
        private IGameStateService _gameStateService;

        public void RegisterGameStateService(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        private void Start()
        {
            Log.Info("Service manger starting...");
            _gameStateService.Start();
        }

        private void Update()
        {
            _gameStateService.Update();
        }

        private void OnDestroy()
        {
            _gameStateService?.Stop();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _gameStateService?.ApplicationPause(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            _gameStateService?.ApplicationQuit();
        }
    }
}