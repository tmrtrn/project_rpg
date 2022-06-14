using Core.Services.Event;

namespace Core.Events
{
    public class ChangeSceneEvent : IGameEvent
    {
        public readonly string sceneName;

        public ChangeSceneEvent(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }
}