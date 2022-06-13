using Core.Services.Event;

namespace Core.Events.GameEvents
{
    public class PlayingStateChangedEvent : IGameEvent
    {
        public readonly bool isPlayerTurn;
        public readonly bool isGameOver;

        public PlayingStateChangedEvent(bool isPlayerTurn, bool isGameOver)
        {
            this.isPlayerTurn = isPlayerTurn;
            this.isGameOver = isGameOver;
        }
    }
}