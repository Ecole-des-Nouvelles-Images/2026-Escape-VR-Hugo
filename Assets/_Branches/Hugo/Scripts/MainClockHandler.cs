using UnityEngine;

namespace _Branches.Hugo.Scripts
{
    public class MainClockHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private bool _reverseRotation = true;
        
        // [Header("===== SETTINGS =====")]
        // [SerializeField] private bool _grabHourHand;
        // [SerializeField] private bool _grabMinuteHand;
        
        // private float _lastMinuteAngle;
        //
        // private void Update()
        // {
        //     if (_grabHourHand || _grabMinuteHand)
        //     {
        //         ClockTimeManager.Instance._playerChangeTime = true;
        //         GetTotalMinutesFromHands();
        //     }
        //     else
        //     {
        //         ClockTimeManager.Instance._playerChangeTime = false;
        //     }
        // }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            ClockTimeManager.Instance.OnTimeChanged += InstanceOnOnTimeChanged;
        }
        
        private void OnDisable()
        {
            ClockTimeManager.Instance.OnTimeChanged -= InstanceOnOnTimeChanged;
        }

        private void InstanceOnOnTimeChanged(float currentNormalized)
        {
            // if (_grabHourHand || _grabMinuteHand) return;
            
            float totalMin = ClockTimeManager.Instance.TotalMinutes;

            float minuteAngle = (totalMin % 60f) * 6f;
            float hourAngle = (totalMin % 720f) * 0.5f;
            
            float direction = _reverseRotation ? 1f : -1f;

            if (_minuteHand)
                _minuteHand.localRotation = Quaternion.Euler(0, 0, direction * -minuteAngle);
            
            if (_hourHand)
                _hourHand.localRotation = Quaternion.Euler(0, 0, direction * -hourAngle);
        }

        #endregion
        
        // private void GetTotalMinutesFromHands()
        // {
        //     if (!_hourHand || !_minuteHand) return;
        //
        //     float direction = _reverseRotation ? 1f : -1f;
        //     float cleanHourAngle = Mathf.Repeat(direction * -_hourHand.localEulerAngles.z, 360f);
        //     float cleanMinuteAngle = Mathf.Repeat(direction * -_minuteHand.localEulerAngles.z, 360f);
        //
        //     float currentTotal = ClockTimeManager.Instance.TotalMinutes;
        //     float newTotalMinutes = currentTotal;
        //
        //     if (_grabHourHand)
        //     {
        //         newTotalMinutes = cleanHourAngle * 2f;
        //         if (newTotalMinutes < 360f) newTotalMinutes += 720f;
        //
        //         float minutesOnly = (newTotalMinutes % 60f) * 6f;
        //         _minuteHand.localRotation = Quaternion.Euler(0, 0, direction * -minutesOnly);
        //     }
        //     else if (_grabMinuteHand)
        //     {
        //         float angleDelta = Mathf.DeltaAngle(_lastMinuteAngle, cleanMinuteAngle);
        //
        //         float minuteDelta = angleDelta / 6f;
        //
        //         newTotalMinutes += minuteDelta;
        //
        //         float hourAngle = (newTotalMinutes % 720f) * 0.5f;
        //         _hourHand.localRotation = Quaternion.Euler(0, 0, direction * -hourAngle);
        //     }
        //
        //     _lastMinuteAngle = cleanMinuteAngle;
        //
        //     ClockTimeManager.Instance.SetTimeManually(newTotalMinutes);
        // }
    }
}