using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Data
{
    /// <summary>
    /// Describes an object that knows how to load all data for the game.
    /// </summary>
    public interface IGameDataService : IService
    {
        IEnumerable<T> LoadResourceGameAsset<T>(string path) where T : Object;
        void SaveModel(string key, ISerializeModel model);
        bool ReadModel<T>(string key, out T model) where T : ISerializeModel;
    }
}