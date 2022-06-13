using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Renders
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _damageText;
        [SerializeField]
        private float _duration;

        private Transform _transform;


        private Transform TransformThis
        {
            get
            {
                if (_transform == null)
                    _transform = transform;
                return _transform;
            }
        }

        public void MakeMove(Vector3 targetPos, float damage, Action cb = null)
        {
            gameObject.SetActive(true);
            TransformThis.DOKill();
            _damageText.text =  $"{damage:0.#}";
            TransformThis.position = targetPos;
            TransformThis.DOMove(new Vector3(0, 50, 0) , _duration).SetRelative(true)
                .SetEase(Ease.Linear).onComplete += () =>
            {
                gameObject.SetActive(false);
                cb?.Invoke();
            };
        }
    }
}