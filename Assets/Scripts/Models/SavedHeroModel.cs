using System;

namespace Models
{
    [Serializable]
    public class SavedHeroModel
    {
        /// <summary>
        /// to bound saved data
        /// </summary>
        public string Id;

        // Min level must be 1
        public int level;

        public int experience;

        public float battleHp;
    }
}