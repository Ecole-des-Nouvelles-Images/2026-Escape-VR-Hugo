using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class AnimationPanel : MonoBehaviour
    {
        public TypeAnimation AnimationType;
    
        [Header("Slide")]
        RectTransform _rectTransform;
        [SerializeField] Vector4 _startPosition;
        [SerializeField] Vector4 _endPosition;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            switch (AnimationType)
            {
                case TypeAnimation.Scale:
                    Vector3 scale = transform.localScale;
                    scale.y = 0;
                    transform.localScale = scale;
        
                    transform.DOScaleY(1, 0.5f).SetUpdate(true);
                    break;

                case TypeAnimation.Slide:
                    _rectTransform.offsetMin = new Vector2(_startPosition.x, _rectTransform.offsetMin.y);
                    _rectTransform.offsetMax = new Vector2(-_startPosition.y, _rectTransform.offsetMax.y);

                    DOTween.To(() => _rectTransform.offsetMin, x => _rectTransform.offsetMin = x,
                        new Vector2(_endPosition.x, _rectTransform.offsetMin.y), 0.5f).SetEase(Ease.OutQuint).SetUpdate(true);
                
                    DOTween.To(() => _rectTransform.offsetMax, x => _rectTransform.offsetMax = x, 
                        new Vector2(-_endPosition.y, _rectTransform.offsetMax.y), 0.5f).SetEase(Ease.OutQuint).SetUpdate(true);
                    break;
            }
        
        }

        public enum TypeAnimation
        {
            Scale,
            Slide
        };
    }
}
