using System;
using Core.Events;
using Core.Services.Event;
using Core.Services.Logging;
using Data.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Renders
{
    public class HeroCardView : MonoBehaviour
    {
        // wait for input down for 3 sec
        private const float HoldTimeThresholdInSec = 3.0f;

        [SerializeField] private Image _selectionImage;
        [SerializeField] private Image _avatar;
        [SerializeField] private Image _frame;
        [SerializeField] private TMP_Text _name;

        private IHeroAsset _assetObject;
        private IEventDispatcher _eventService;
        private float _inputHoldTimeout;

        protected bool isOpponentCard = false;

        public void Render(IHeroAsset heroAsset, IEventDispatcher eventService, bool inTeam)
        {
            _assetObject = heroAsset;
            _eventService = eventService;
            _avatar.sprite = _assetObject.ViewAsset.View;
            _frame.color = _assetObject.ViewAsset.BackgroundColor;

            _name.text = _assetObject.Attributes.Name;

            if (inTeam)
            {
                Select();
            }
            else
            {
                Deselect();
            }
        }

        public string GetHeroId()
        {
            return _assetObject.Id;
        }

        public void Select()
        {
            Color color = _selectionImage.color;
            color.a = 1;
            _selectionImage.color = color;
        }

        public void Deselect()
        {
            Color color = _selectionImage.color;
            color.a = 0;
            _selectionImage.color = color;
        }

        #region Input Handling

        /// <summary>
        /// Call when input is first down.
        /// </summary>
        public void InputDown()
        {
            Log.Debug("Input down");
            _inputHoldTimeout = Time.time + 3f;
        }

        /// <summary>
        /// Called on the up frame.
        /// </summary>
        public void InputUp()
        {
            if (_inputHoldTimeout > 0) // cancel holding, the action is tap
            {
                _inputHoldTimeout = 0;
                Log.Info("Input->tap card view");
                _eventService.Publish(new HeroCardInputEvent(HeroCardInputEvent.InputType.Tap, this, isOpponentCard));
            }
        }

        #endregion

        void Update()
        {
            if (_inputHoldTimeout > 0 && Time.time > _inputHoldTimeout)
            {
                Log.Info("Input->hold card view ");
                _inputHoldTimeout = 0;
                _eventService.Publish(new HeroCardInputEvent(HeroCardInputEvent.InputType.Hold, this, isOpponentCard));
            }
        }

        protected Image GetAvatarImage()
        {
            return _avatar;
        }
    }
}