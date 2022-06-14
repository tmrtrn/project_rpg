using Core.Services.Event;
using Renders;

namespace Core.Events
{
    public class HeroCardInputEvent : IGameEvent
    {
        public enum InputType
        {
            Tap,
            Hold
        }

        public readonly InputType inputType;
        public readonly HeroCardView card;
        public readonly bool isOpponentCard;

        public HeroCardInputEvent(InputType inputType, HeroCardView card, bool isOpponentCard = false)
        {
            this.inputType = inputType;
            this.card = card;
            this.isOpponentCard = isOpponentCard;
        }
    }
}