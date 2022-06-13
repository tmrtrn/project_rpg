using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Core.Events;
using Core.Services.Data;
using Core.Services.Event;
using Core.Services.Logging;
using Data.Hero;
using Models;
using Utils;

namespace Core
{
    public sealed class GameController : IGameController
    {
        private readonly IGameDataService _dataService;
        private readonly IEventDispatcher _eventDispatcher;

        /// <summary>
        /// Keeps all hero assets
        /// </summary>
        private Dictionary<string, IHeroAsset> _heroAssets;
        private RuntimeGameModel _runtimeGame;

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
            var heroAssetObjects =_dataService.LoadResourceGameAsset<HeroAssetObject>("GameAssets/Hero");
            _heroAssets = new Dictionary<string, IHeroAsset>(10);
            foreach (HeroAssetObject heroAssetObject in heroAssetObjects)
            {
                _heroAssets.Add(heroAssetObject.Id, heroAssetObject);
            }

            // TODO: load saved game process as well
            _eventDispatcher.Publish(new PreLoadAssetsEvent());
        }

        public void GenerateRuntimeData()
        {
            if (_dataService.ReadModel("progress", out SavedGameModel progress))
            {
                try
                {
                    _runtimeGame = new RuntimeGameModel(progress, this);
                    return;
                }
                catch (Exception ex)
                {
                    Log.Error("Failed while initializing the runtime game model, Fallback strategy is create a new saved file ", ex);
                }
            }

            // new gamer
            progress = GenerateNewGame();
            _runtimeGame = new RuntimeGameModel(progress, this);
        }

        /// <summary>
        /// Generates the persistence file to keep game state and hero collections as well
        /// Calls when player load the game first time or hero db corrupted
        /// </summary>
        /// <returns></returns>
        private SavedGameModel GenerateNewGame()
        {
            var savedGameModel = new SavedGameModel();

            // create initial heroes
            int max = Math.Min(_heroAssets.Count, GameConstants.InitialHeroCount);

            if (max <= 0) throw new Exception("Define heroes as scriptable object or set min hero count correctly");

            List<string> allHeroIds = _heroAssets.Keys.ToList();
            // shuffle the list and select ${max} heroes
            RandomUtil.Shuffle(allHeroIds);

            // create user's hero collection
            savedGameModel.heroCollection = new List<SavedHeroModel>(allHeroIds.Count);
            for (int i = 0; i < max; i++)
            {
                GetHeroAssetById(allHeroIds[i], out var heroAsset);
                savedGameModel.heroCollection.Add(CreateSavedHeroData(heroAsset));
            }

            int requiredHeroCountToFight = Math.Min(GameConstants.InitialHeroCount, allHeroIds.Count);

            // create a empty team for player
            savedGameModel.playerTeam = new string[requiredHeroCountToFight];


            return savedGameModel;
        }

        public RuntimeGameModel GetRuntimeState()
        {
            return _runtimeGame;
        }

        private SavedHeroModel CreateSavedHeroData(IHeroAsset assetObject)
        {
            return new SavedHeroModel
            {
                Id = assetObject.Id,
                experience = 0,
                level = 1
            };
        }

        public bool GetHeroAssetById(string id, out IHeroAsset heroData)
        {
            return _heroAssets.TryGetValue(id, out heroData);
        }


        /// <summary>
        /// Gets a random asset different than player's inventory
        /// If there is no different hero, just picks a random hero and returns false
        /// </summary>
        /// <param name="excludePlayerTeam"></param>
        /// <param name="excludeEnemyTeam"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool GetRandomHeroAsset(bool excludePlayerTeam, bool excludeEnemyTeam, out IHeroAsset asset)
        {
            if (_heroAssets.Count == 0) throw new Exception("create a hero asset in resources folder first");
            List<string> allHeroIds = _heroAssets.Keys.ToList();
            RandomUtil.Shuffle(allHeroIds);

            for (int i = 0; i < allHeroIds.Count; i++)
            {
                if ((!excludePlayerTeam || !_runtimeGame.IsHeroInPlayerTeam(allHeroIds[i])) &&
                    (!excludeEnemyTeam || !_runtimeGame.IsHeroInEnemyTeam(allHeroIds[i])))
                {
                    // we found
                    return GetHeroAssetById(allHeroIds[i], out asset);
                }
            }
            // all hero assets are listing in player team or enemy team
            // just pick a random asset and return false
            GetHeroAssetById(allHeroIds[0], out asset);
            return false;
        }

        public void CreateNewBattle()
        {

        }


    }
}