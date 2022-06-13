using Core.Services.Event;
using Models;
using UnityEngine;

namespace Renders
{
    public interface IHeroBattleCardView
    {
        void Render(HeroModel heroModel, IEventDispatcher eventService);
        void UpdateHealth();
        Transform TargetPoint { get; }
        void Select();
        void Deselect();
    }
}