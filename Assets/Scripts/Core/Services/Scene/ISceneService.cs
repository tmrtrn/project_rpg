using System;

namespace Core.Services.Scene
{
    public interface ISceneService : IService
    {
        /// <summary>
        /// loads a scene with given name
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public void LoadScene(string scene);
    }
}