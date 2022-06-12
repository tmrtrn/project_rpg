using System;
using System.Collections.Generic;
using Core.Services.Logging;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Services.Data
{
    public class GameDataService : IGameDataService
    {
        public IEnumerable<T> LoadResourceGameAsset<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }

        public void SaveModel(string key, ISerializeModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            string data = JsonUtility.ToJson(model);
            PlayerPrefs.SetString(key, data);
        }

        public bool ReadModel<T>(string key, out T model) where T : ISerializeModel
        {
            model = default;
            string data = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }

            try
            {
                model = JsonUtility.FromJson<T>(data);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception while reading data {data}", ex);
                throw;
            }
        }

    }
}