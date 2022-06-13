using Core.Services.Event;

namespace Core.Events.GameEvents
{
    public class PlayingStateStartedEvent : IGameEvent
    {
        public readonly bool isPlayerTurn;

        public PlayingStateStartedEvent(bool isPlayerTurn)
        {
            this.isPlayerTurn = isPlayerTurn;
        }
    }
}