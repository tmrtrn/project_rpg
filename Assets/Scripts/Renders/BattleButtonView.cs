using System;
using UnityEngine;
using UnityEngine.UI;

namespace Renders
{
    public class BattleButtonView : MonoBehaviour
    {
        [SerializeField] public Button _button;
        public void SetReady(bool active)
        {
            _button.interactable = active;
        }
    }
}