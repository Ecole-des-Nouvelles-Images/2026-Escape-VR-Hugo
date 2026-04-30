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
        [SerializeField] private float _minTimeMinutes = 6f * 60f;
        [SerializeField] private float _maxTimeMinutes = 18f * 60f;
        [SerializeField] private float _timeUpdateThreshold = 0.0001f;

        [Header("===== DEBUG =====")]
        public float TotalMinutes;

        private float _lastNotifiedTime = -1f;

        private void Awake()
        {
            TotalMinutes = _minTimeMinutes;
        }

        private void Update()
        {
            TotalMinutes += Time.deltaTime * _timeSpeedMultiplier;
            TotalMinutes = Mathf.Clamp(TotalMinutes, _minTimeMinutes, _maxTimeMinutes);

            float currentNormalized = Mathf.InverseLerp(_minTimeMinutes, _maxTimeMinutes, TotalMinutes);
            
            if (Mathf.Abs(currentNormalized - _lastNotifiedTime) > _timeUpdateThreshold)
            {
                _lastNotifiedTime = currentNormalized;
                OnTimeChanged?.Invoke(currentNormalized);
            }
        }

        public void SetTimeManually(float totalMinutes)
        {
            TotalMinutes = Mathf.Clamp(totalMinutes, _minTimeMinutes, _maxTimeMinutes);
        }
    }
}