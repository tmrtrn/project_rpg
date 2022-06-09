using UnityEngine;

namespace Core.Services
{
    public static class AutoInitServiceLayer
    {
        /// <summary>
        /// Initialize services before the scene loaded
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnLoadBeforeScene()
        {
            GameObject gameObject = Object.Instantiate(new GameObject());
            gameObject.name = "ServiceManager";
            gameObject.AddComponent<ServiceManager>();
            Object.DontDestroyOnLoad(gameObject);
        }
    }
}