using System;
using System.Collections.Generic;
using Core;
using Core.Services.Logging;
using Data.Hero;
using DefaultNamespace;

namespace Models
{
    public class RuntimeGameModel
    {
        private SavedGameModel _savedGameModel;
        public List<HeroModel> HeroCollection { get; private set; }

        public RuntimeGameModel(SavedGameModel savedGameModel, GameController gameController)
        {
            _savedGameModel = savedGameModel;
            if (_savedGameModel == null)
            {
                throw new NullReferenceException("Saved game progress is null");
            }

            HeroCollection = new List<HeroModel>(savedGameModel.heroCollection.Count);

            // Generate player's hero collection
            foreach (SavedHeroModel savedHeroModel in savedGameModel.heroCollection)
            {
                if (!gameController.GetHeroAssetById(savedHeroModel.Id, out var heroAsset))
                {
                    Log.Error($"Hero Db may be corrupted, {savedHeroModel.Id} ");
                    throw new Exception("Hero Db may be corrupted");
                }
                AddHeroToPlayerCollection(heroAsset, savedHeroModel);
            }
        }

        public IHeroModelAttribute GetHeroModelAttribute(string id)
        {
            foreach (HeroModel model in HeroCollection)
            {
                if (model.Id.Equals(id))
                {
                    return model;
                }
            }

            throw new Exception("Hero model with id not found");
        }

        public HeroTeam GetPlayerTeam()
        {
            return _savedGameModel.playerTeam;
        }

        private void AddHeroToPlayerCollection(IHeroAsset heroAsset, SavedHeroModel savedHeroModel)
        {
            if (HeroCollection.Count == GameConstants.InitialHeroCount)
            {
                throw new Exception($"can not exceed the {GameConstants.InitialHeroCount}");
            }
            HeroModel hero = new HeroModel(heroAsset, savedHeroModel);
            HeroCollection.Add(hero);
        }

        public bool IsHeroInPlayerTeam(string id)
        {
            return _savedGameModel.playerTeam.IsMember(id);
        }


        /// <summary>
        /// returns true if the hero is added or removed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool AddOrRemoveFromTeam(string id)
        {
            if (_savedGameModel.playerTeam.IsMember(id))
            {
                return _savedGameModel.playerTeam.RemoveFromTeam(id);
            }

            // we are about to add to the team, but first check there is a available seat for her
            if (_savedGameModel.playerTeam.IsFull())
            {
                // no we don't have, also we are ready to the battle
                return false;
            }

            // seems like we can add to the team
            return _savedGameModel.playerTeam.AddHeroToTeam(id);
        }

        public bool IsReadyToFight()
        {
            return _savedGameModel.playerTeam.IsFull();
        }
    }
}