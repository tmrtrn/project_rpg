using Data.Hero;
using Models;
using TMPro;
using UnityEngine;

namespace Renders
{
    public class InfoPopUpView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _attackPoint;
        [SerializeField] private TMP_Text _experience;

        public void Render(IHeroModelAttribute heroAttributes)
        {
            _name.text = heroAttributes.Name;
            _level.text = $"{heroAttributes.Level}";
            _health.text = heroAttributes.FullHealth.ToString("0.#");
            _attackPoint.text = heroAttributes.FullAttack.ToString("0.#");
            _experience.text = $"{heroAttributes.Experience}";

            gameObject.SetActive(true);
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}