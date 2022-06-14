using System;
using Data.Hero;
using Models;

namespace Core
{
    public interface IGameController
    {
        public void PreloadAssets();
        public void SavePlayerProgress();
        bool GetHeroAssetById(string id, out IHeroAsset heroData);
        RuntimeGameModel GetRuntimeState();
        SavedHeroModel CreateSavedHeroData(IHeroAsset assetObject);
        bool GetRandomHeroAsset(bool excludePlayerTeam, bool excludeEnemyTeam, out IHeroAsset asset);
        bool FindNewHeroAssetExceptInventory(out IHeroAsset asset);
        bool HasActiveBattle();
        void GenerateRuntimeData();
    }
}