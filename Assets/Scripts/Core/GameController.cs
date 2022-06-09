using System;
using Core.Events;
using Core.Services.Data;
using Core.Services.Event;
using Core.Services.Logging;
using Data.Hero;

namespace Core
{
    public sealed class GameController : IGameController
    {
        private readonly IGameDataService _dataService;
        private readonly IEventDispatcher _eventDispatcher;

        public GameController(IGameDataService dataService, IEventDispatcher eventDispatcher)
        {
            _dataService = dataService;
            _eventDispatcher = eventDispatcher;
        }

        /// <summary>
        /// just give an insight of how async calls can be executed
        /// </summary>
        /// <returns></returns>
        public void PreloadAssets()
        {
            var heroAssets =_dataService.LoadResourceGameAsset<HeroAssetObject>("GameAssets/Hero");
            // TODO: load saved game process as well
            _eventDispatcher.Publish(new PreLoadAssetsEvent());
        }


    }
}