using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Data
{
    public class GameDataService : IGameDataService
    {
        public IEnumerable<T> LoadResourceGameAsset<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }
    }
}