using Core.Services.Event;
using Data.Hero;
using UnityEngine;

namespace Renders
{
    public class GameResultSectionPanel : MonoBehaviour
    {
        [SerializeField] private Transform panelTransform;
        [SerializeField] private GameObject heroPrefabObject;

        private IEventDispatcher _eventService;

        public void Inject(IEventDispatcher eventService)
        {
            _eventService = eventService;
        }

        public void SetVisible(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void CreateCard(IHeroAsset asset)
        {
            GameObject heroViewObj = Instantiate(heroPrefabObject, panelTransform);
            HeroCardView card = heroViewObj.GetComponent<HeroCardView>();
            card.Render(asset, _eventService, false);
        }
    }
}