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

        public HeroCardInputEvent(InputType inputType, HeroCardView card)
        {
            this.inputType = inputType;
            this.card = card;
        }
    }
}