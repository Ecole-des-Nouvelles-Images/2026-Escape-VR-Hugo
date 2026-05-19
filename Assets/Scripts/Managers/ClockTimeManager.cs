using System;
using Core.Singletons;
using UnityEngine;

namespace Managers
{
    public class ClockTimeManager : MonoBehaviourSingleton<ClockTimeManager>
    {
        public event Action<float> OnTimeChanged;
        public float NormalizedCurrentTime { get; private set; }

        [Header("===== CONFIG =====")]
        [SerializeField] private float _timeSpeedMultiplier = 1f;
        [SerializeField] private float _minTimeMinutes = 6f * 60f;
        [SerializeField] private float _maxTimeMinutes = 18f * 60f;
        [SerializeField] private float _timeUpdateThreshold = 0.0001f;

        [Header("===== DEBUG =====")]
        public float TotalMinutes;
        public bool IsPaused;

        private float _lastNotifiedTime = -1f;
        
        public void SetTimeManually(float totalMinutes)
        {
            TotalMinutes = Mathf.Clamp(totalMinutes, _minTimeMinutes, _maxTimeMinutes);
            UpdateNormalizedTime();
        }

        private void Update()
        {
            if (IsPaused) return;
            
            TotalMinutes += Time.deltaTime * _timeSpeedMultiplier;
            TotalMinutes = Mathf.Clamp(TotalMinutes, _minTimeMinutes, _maxTimeMinutes);
            
            UpdateNormalizedTime();
        }

        private void UpdateNormalizedTime()
        {
            NormalizedCurrentTime = Mathf.InverseLerp(_minTimeMinutes, _maxTimeMinutes, TotalMinutes);
            
            if (Mathf.Abs(NormalizedCurrentTime - _lastNotifiedTime) > _timeUpdateThreshold)
            {
                _lastNotifiedTime = NormalizedCurrentTime;
                OnTimeChanged?.Invoke(NormalizedCurrentTime);
            }
        }
    }
}