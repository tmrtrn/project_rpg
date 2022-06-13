using System;
using System.Collections.Generic;
using Constants;
using Core;
using Core.Services.Logging;
using Data.Hero;
using Random = UnityEngine.Random;

namespace Models
{
    public class RuntimeGameModel
    {
        private readonly GameController _gameController;
        private readonly SavedGameModel _savedGameModel;

        /// <summary>
        /// Player inventory
        /// </summary>
        public List<HeroModel> PlayerHeroCollection { get; private set; }

        /// <summary>
        /// creates in battle
        /// </summary>
        public List<HeroModel> EnemyHeroCollection { get; private set; }

        public RuntimeGameModel(SavedGameModel savedGameModel, GameController gameController)
        {
            _savedGameModel = savedGameModel;
            _gameController = gameController;

            if (_savedGameModel == null)
            {
                throw new NullReferenceException("Saved game progress is null");
            }

            PlayerHeroCollection = new List<HeroModel>(savedGameModel.heroCollection.Count);

            // Generate player's hero collection
            foreach (SavedHeroModel savedHeroModel in savedGameModel.heroCollection)
            {
                if (!_gameController.GetHeroAssetById(savedHeroModel.Id, out var heroAsset))
                {
                    string exception = $"Hero Db may be corrupted, {savedHeroModel.Id} ";
                    Log.Error(exception);
                    throw new Exception(exception);
                }
                AddHeroToPlayerCollection(heroAsset, savedHeroModel);
            }
        }

        public HeroModel GetPlayerHeroModel(string id)
        {
            return GetHeroModel(id, PlayerHeroCollection);
        }

        public HeroModel GetOpponentHeroModel(string id)
        {
            return GetHeroModel(id, EnemyHeroCollection);
        }

        /// <summary>
        /// Checks if hero is in player inventory
        /// </summary>
        /// <param name="heroId"></param>
        /// <returns></returns>
        public bool IsHeroInCollection(string heroId)
        {
            foreach (HeroModel model in PlayerHeroCollection)
            {
                if (model.Id.Equals(heroId))
                {
                    return true;
                }
            }

            return false;
        }

        private HeroModel GetHeroModel(string id, List<HeroModel> modelList)
        {
            foreach (HeroModel model in modelList)
            {
                if (model.Id.Equals(id))
                {
                    return model;
                }
            }

            throw new Exception("Hero model with id not found");
        }
        private HeroModel AddHeroToPlayerCollection(IHeroAsset heroAsset, SavedHeroModel savedHeroModel)
        {
            if (PlayerHeroCollection.Count == GameConstants.PlayerCollectionHeroLimit)
            {
                throw new Exception($"can not exceed the {GameConstants.PlayerCollectionHeroLimit}");
            }
            HeroModel hero = new HeroModel(heroAsset, savedHeroModel);
            PlayerHeroCollection.Add(hero);
            return hero;
        }

        /// <summary>
        /// Finds a new hero model which doesn't exist in player's inventory
        /// returns success if found
        /// </summary>
        /// <returns></returns>
        public bool GenerateAndAddNewHeroModel(out HeroModel newHeroModel)
        {
            if (_gameController.FindNewHeroAssetExceptInventory(out var asset))
            {
                SavedHeroModel savedHeroModel = _gameController.CreateSavedHeroData(asset);
                newHeroModel = AddHeroToPlayerCollection(asset, savedHeroModel);
                return true;
            }
            Log.Warning("Couldn't find any new hero");
            newHeroModel = null;
            return false;
        }


        #region Team Methods


        public string[] GetPlayerTeam()
        {
            return _savedGameModel.playerTeam;
        }

        public string[] GetEnemyTeam()
        {
            return _savedGameModel.enemyTeam;
        }

        public bool IsHeroInPlayerTeam(string id)
        {
            return _savedGameModel.IsPlayerTeamMember(id);
        }

        public bool IsHeroInEnemyTeam(string id)
        {
            if (_savedGameModel.enemyTeam == null) return false;
            return _savedGameModel.IsEnemyTeamMember(id);
        }


        /// <summary>
        /// returns true if the hero is added or removed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool AddOrRemoveFromTeam(string id)
        {
            if (_savedGameModel.IsPlayerTeamMember(id))
            {
                return _savedGameModel.RemoveFromPlayerTeam(id);
            }

            // we are about to add to the team, but first check there is a available seat for her
            if (_savedGameModel.IsPlayerTeamFull())
            {
                // no we don't have, also we are ready to the battle
                return false;
            }

            // seems like we can add to the team
            return _savedGameModel.AddHeroToPlayerTeam(id);
        }

        /// <summary>
        /// Returns player's min and max hero levels
        /// </summary>
        /// <returns></returns>
        public (int, int) GetPlayerTeamLevelBounds()
        {
            int min = 1;
            int max = 1;
            foreach (string member in _savedGameModel.playerTeam)
            {
                if (string.IsNullOrEmpty(member)) continue;
                HeroModel model = PlayerHeroCollection.Find(x => x.Id.Equals(member));
                if (model.Level < min)
                {
                    min = model.Level;
                }

                if (model.Level > max)
                {
                    max = model.Level;
                }
            }

            return (min, max);
        }

        #endregion

        #region Battle

        public bool HasActiveBattle()
        {
            return _savedGameModel.IsPlayingBattle;
        }

        public void CreateNewBattle()
        {
            // Create enemy team with random heroes by given count in constants
            int capacity = GameConstants.EnemyHeroCount;
            _savedGameModel.enemyTeam = new string[capacity];

            // create hero collection and it's team
            EnemyHeroCollection = new List<HeroModel>(capacity);
            for (int i = 0; i < _savedGameModel.enemyTeam.Length; i++)
            {
                bool found = _gameController.GetRandomHeroAsset(true,  true, out var heroAsset);
                if (!found)
                {
                    Log.Warning("couldn't find new hero");
                }

                // select opponent's level by the player's team
                (int minLevel, int maxLevel) = GetPlayerTeamLevelBounds();
                int enemyLevel = Random.Range(minLevel, maxLevel + 1);
                // first add to enemy collection to keep data
                var model = new HeroModel(heroAsset, new SavedHeroModel()
                {
                    Id = heroAsset.Id,
                    experience = 0,
                    level = enemyLevel,
                });
                model.ResetHpForBattle();
                EnemyHeroCollection.Add(model);
                _savedGameModel.enemyTeam[i] = heroAsset.Id;
            }

            // reset player hero healths
            for (int i = 0; i < _savedGameModel.playerTeam.Length; i++)
            {
                GetPlayerHeroModel(_savedGameModel.playerTeam[i]).ResetHpForBattle();
            }

            _savedGameModel.Reset();
            SetBattleStarted();

            // set whois turn
            _savedGameModel.EndTurn();
        }

        public bool IsTurnOver()
        {
            return _savedGameModel.IsTurnOver();
        }

        public void EndTurn()
        {
            _savedGameModel.EndTurn();
        }

        public void SetPlayerMoveSuccess()
        {
            _savedGameModel.SetMoveSuccess();
        }

        public bool IsGameOver()
        {
            return !IsAnyAlivePlayerTeamMember() || !IsAnyAliveOpponentMember();
        }

        public bool IsAnyAlivePlayerTeamMember()
        {
            string[] playerTeam = GetPlayerTeam();
            for (int i = 0; i < playerTeam.Length; i++)
            {
                if (GetPlayerHeroModel(playerTeam[i]).GetCurrentHp() > 0) return true;
            }

            return false;
        }

        public bool IsAnyAliveOpponentMember()
        {
            string[] team = GetEnemyTeam();
            for (int i = 0; i < team.Length; i++)
            {
                if (GetOpponentHeroModel(team[i]).GetCurrentHp() > 0) return true;
            }

            return false;
        }

        public HeroModel FindRandomPlayerHero()
        {
            List<HeroModel> aliveMembers = GetAlivePlayerHeroes();
            if (aliveMembers.Count == 0)
            {
                throw new Exception("couldn't find an alive member");
            }

            return aliveMembers[Random.Range(0, aliveMembers.Count)];
        }

        public HeroModel FindRandomOpponentHero()
        {
            List<HeroModel> aliveMembers = GetAliveOpponentHeroes();
            if (aliveMembers.Count == 0)
            {
                throw new Exception("couldn't find an alive member");
            }

            return aliveMembers[Random.Range(0, aliveMembers.Count)];
        }

        public List<HeroModel> GetAlivePlayerHeroes()
        {
            string[] playerTeam = GetPlayerTeam();
            List<HeroModel> aliveMembers = new List<HeroModel>(playerTeam.Length);
            for (int i = 0; i < playerTeam.Length; i++)
            {
                HeroModel member = GetPlayerHeroModel(playerTeam[i]);
                if (member.GetCurrentHp() > 0)
                {
                    aliveMembers.Add(member);
                }
            }

            return aliveMembers;
        }

        public List<HeroModel> GetAliveOpponentHeroes()
        {
            string[] team = GetEnemyTeam();
            List<HeroModel> aliveMembers = new List<HeroModel>(team.Length);
            for (int i = 0; i < team.Length; i++)
            {
                HeroModel member = GetOpponentHeroModel(team[i]);
                if (member.GetCurrentHp() > 0)
                {
                    aliveMembers.Add(member);
                }
            }

            return aliveMembers;
        }

        /// <summary>
        /// First one is player
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerTurn()
        {
            return _savedGameModel.WhoisTurn == 0;
        }

        #endregion


        public bool IsReadyToFight()
        {
            return _savedGameModel.IsPlayerTeamFull();
        }

        public void SetBattleStarted()
        {
            _savedGameModel.IsPlayingBattle = true;
        }

        public void SetBattleOver()
        {
            _savedGameModel.PlayedBattleCount++;
            _savedGameModel.Reset();
        }

        public int GetPlayedBattleCount()
        {
            return _savedGameModel.PlayedBattleCount;
        }
    }
}