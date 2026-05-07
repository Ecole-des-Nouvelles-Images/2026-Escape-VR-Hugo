using UnityEngine;

namespace _Branches.Hugo.Scripts
{
    public abstract class TemporalGameObject : MonoBehaviour
    {
        [Header("===== TEMPORAL =====")]
        [SerializeField] protected Vector2 _temporalRange = new(0f, 1f);
        [SerializeField, Range(0f, 1f)] protected float _state;
        
        protected virtual void OnEnable()
        {
            if (ClockTimeManager.Instance != null)
                ClockTimeManager.Instance.OnTimeChanged += OnTimeChanged;
        }
        
        protected virtual void OnDisable()
        {
            if (ClockTimeManager.Instance != null)
                ClockTimeManager.Instance.OnTimeChanged -= OnTimeChanged;
        }

        private void OnTimeChanged(float currentTime)
        {
            _state = Mathf.InverseLerp(_temporalRange.x, _temporalRange.y, currentTime);
            
            TimeBehavior();
        }

        protected abstract void TimeBehavior();
    }
}