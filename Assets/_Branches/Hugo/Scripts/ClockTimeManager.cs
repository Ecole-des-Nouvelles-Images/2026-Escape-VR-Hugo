using System;
using Core.Singletons;
using UnityEngine;

namespace _Branches.Hugo.Scripts
{
    public class ClockTimeManager : MonoBehaviourSingleton<ClockTimeManager>
    {
        public event Action<float> OnTimeChanged;

        [Header("===== CONFIG =====")]
        [SerializeField] private float _timeSpeedMultiplier = 1f;
        [SerializeField] private float _minTimeMinutes = 6f * 60f; // 360
        [SerializeField] private float _maxTimeMinutes = 18f * 60f; // 1080

        [Header("===== DEBUG =====")]
        [SerializeField] private float _totalMinutes;

        private float _lastNotifiedTime = -1f;

        private void Awake() => _totalMinutes = _minTimeMinutes;

        private void Update()
        {
            _totalMinutes += Time.deltaTime * _timeSpeedMultiplier;
            _totalMinutes = Mathf.Clamp(_totalMinutes, _minTimeMinutes, _maxTimeMinutes);

            float currentNormalized = Mathf.InverseLerp(_minTimeMinutes, _maxTimeMinutes, _totalMinutes);
            
            if (Mathf.Abs(currentNormalized - _lastNotifiedTime) > 0.0001f)
            {
                _lastNotifiedTime = currentNormalized;
                OnTimeChanged?.Invoke(currentNormalized);
            }
        }

        public void SetTimeManually(float totalMinutes)
        {
            _totalMinutes = Mathf.Clamp(totalMinutes, _minTimeMinutes, _maxTimeMinutes);
        }
    }
}