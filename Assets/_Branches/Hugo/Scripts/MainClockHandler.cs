using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace _Branches.Hugo.Scripts
{
    public class MainClockHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private bool _reverseRotation;

        [Header("===== INTERACTION =====")]
        [SerializeField] private XRGrabInteractable _hourGrab;
        [SerializeField] private XRGrabInteractable _minuteGrab;

        [Header("===== DEBUG =====")]
        [SerializeField] private bool _grabHourHand;
        [SerializeField] private bool _grabMinuteHand;
        [SerializeField] private float _lastMinuteAngle;

        private void Update()
        {
            if (_grabHourHand || _grabMinuteHand)
            {
                GetTotalMinutesFromHands();
            }
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            ClockTimeManager.Instance.OnTimeChanged += InstanceOnOnTimeChanged;

            if (_hourGrab) 
            {
                _hourGrab.selectEntered.AddListener(x => OnGrabHand(true, true));
                _hourGrab.selectExited.AddListener(x => OnGrabHand(true, false));
            }
            
            if (_minuteGrab) 
            {
                _minuteGrab.selectEntered.AddListener(x => OnGrabHand(false, true));
                _minuteGrab.selectExited.AddListener(x => OnGrabHand(false, false));
            }
        }

        private void OnDisable()
        {
            ClockTimeManager.Instance.OnTimeChanged -= InstanceOnOnTimeChanged;

            if (_hourGrab) _hourGrab.selectEntered.RemoveAllListeners();
            if (_minuteGrab) _minuteGrab.selectEntered.RemoveAllListeners();
        }
        
        private void InstanceOnOnTimeChanged(float currentNormalized)
        {
            if (_grabHourHand || _grabMinuteHand) return;
            
            float totalMin = ClockTimeManager.Instance.TotalMinutes;
            float direction = _reverseRotation ? 1f : -1f;

            if (_minuteHand)
                _minuteHand.localRotation = Quaternion.Euler(0, 0, direction * -(totalMin % 60f * 6f));
            
            if (_hourHand)
                _hourHand.localRotation = Quaternion.Euler(0, 0, direction * -(totalMin % 720f * 0.5f));
        }

        private void OnGrabHand(bool isHourHand, bool isGrabbed)
        {
            if (isHourHand) _grabHourHand = isGrabbed;
            else _grabMinuteHand = isGrabbed;

            ClockTimeManager.Instance.IsPaused = _grabHourHand || _grabMinuteHand;

            if (isGrabbed && !isHourHand)
            {
                float direction = _reverseRotation ? 1f : -1f;
                _lastMinuteAngle = Mathf.Repeat(direction * -_minuteHand.localEulerAngles.z, 360f);
            }
        }

        private void GetTotalMinutesFromHands()
        {
            float direction = _reverseRotation ? 1f : -1f;
            float cleanHourAngle = Mathf.Repeat(direction * -_hourHand.localEulerAngles.z, 360f);
            float cleanMinuteAngle = Mathf.Repeat(direction * -_minuteHand.localEulerAngles.z, 360f);

            float newTotalMinutes = ClockTimeManager.Instance.TotalMinutes;

            if (_grabHourHand)
            {
                newTotalMinutes = cleanHourAngle * 2f;
                if (newTotalMinutes < 360f) newTotalMinutes += 720f;

                _minuteHand.localRotation = Quaternion.Euler(0, 0, direction * -(newTotalMinutes % 60f * 6f));
            }
            else if (_grabMinuteHand)
            {
                float angleDelta = Mathf.DeltaAngle(_lastMinuteAngle, cleanMinuteAngle);
                newTotalMinutes += angleDelta / 6f;
                _lastMinuteAngle = cleanMinuteAngle;

                _hourHand.localRotation = Quaternion.Euler(0, 0, direction * -(newTotalMinutes % 720f * 0.5f));
            }

            ClockTimeManager.Instance.SetTimeManually(newTotalMinutes);
        }

        #endregion
    }
}