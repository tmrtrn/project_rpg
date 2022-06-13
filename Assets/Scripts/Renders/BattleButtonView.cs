using System;
using Core.Services.Event;
using UnityEngine;
using UnityEngine.UI;

namespace Renders
{
    public class BattleButtonView : MonoBehaviour
    {
        private IEventDispatcher _eventService;
        [SerializeField] private Button _button;
        public Action OnClicked;

        public void InjectServices(IEventDispatcher eventService)
        {
            _eventService = eventService;
        }
        public void SetReady(bool active)
        {
            _button.interactable = active;
        }

        public void HandleClick()
        {
            OnClicked?.Invoke();
        }
    }
}