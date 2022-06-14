namespace Core.Services
{
    public interface IGameStateService
    {
        void Start();
        void Update();
        void Stop();
        void ApplicationPause(bool pauseStatus);
        void ApplicationQuit();
    }
}