using Core.Services.Event;

namespace Core.Events
{
    public class SceneLoaded : IGameEvent
    {
        public string SceneName { get; }

        public SceneLoaded(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}