using System;
using UnityEngine;

namespace Data.Hero
{
    [Serializable]
    public class HeroAttributeAsset
    {
        #region Serialized Fields

        [SerializeField]
        private string _name;
        [SerializeField]
        private int _health;
        [SerializeField]
        private int _attack;
        [SerializeField]
        private int _experience;
        [SerializeField]
        private int _level;
        #endregion

        #region Public Implementation


        public string Name => _name;

        public int Health => _health;

        public int Attack => _attack;

        public int Experience => _experience;

        public int Level => _level;

        #endregion
    }
}