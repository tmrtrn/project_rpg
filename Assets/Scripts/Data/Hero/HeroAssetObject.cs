using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Hero
{
    [CreateAssetMenu(fileName = "New Hero", menuName = "Create Hero Asset Object", order = 1)]
    public class HeroAssetObject : ScriptableObject, IHeroAsset
    {
        #region Serialized Properties

        [SerializeField]
        private string _id;

        [SerializeField]
        private HeroAttributeAsset _attributes;

        [SerializeField]
        private HeroViewAsset _viewAsset;

        #endregion

        #region Public Implementation

        public string Id => _id;
        public HeroAttributeAsset Attributes => _attributes;
        public HeroViewAsset ViewAsset => _viewAsset;

        #endregion
    }
}