using System;
using UnityEngine;

namespace Data.Hero
{
    [Serializable]
    public class HeroViewAsset
    {
        #region Serialized Fields

        [SerializeField]
        private Sprite _view;

        [SerializeField]
        private Color _color;


        #endregion

        #region Public Implementation

        public Sprite View => _view;
        public Color BackgroundColor => _color;

        #endregion
    }
}