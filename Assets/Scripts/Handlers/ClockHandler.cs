using Managers;
using UnityEngine;

namespace Handlers
{
    public class ClockHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private bool _reverseRotation;

        #region ===== EVENTS =====

        private void OnEnable()
        {
            if (ClockTimeManager.Instance) ClockTimeManager.Instance.OnTimeChanged += InstanceOnOnTimeChanged;
        }

        private void OnDisable()
        {
            if (ClockTimeManager.Instance) ClockTimeManager.Instance.OnTimeChanged -= InstanceOnOnTimeChanged;
        }

        private void InstanceOnOnTimeChanged(float currentNormalized)
        {
            float totalMin = ClockTimeManager.Instance.TotalMinutes;
            float direction = _reverseRotation ? 1f : -1f;

            if (_minuteHand)
                _minuteHand.localRotation = Quaternion.Euler(0, 0, direction * -(totalMin % 60f * 6f));
            
            if (_hourHand)
                _hourHand.localRotation = Quaternion.Euler(0, 0, direction * -(totalMin % 720f * 0.5f));
        }

        #endregion
    }
}