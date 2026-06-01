using MonoBehiavors;
using UnityEngine;

namespace Handlers
{
    public class TemporalSunHandler : TemporalGameObject
    {
        [Header("===== SUN ROTATION SETTINGS =====")]
        [SerializeField] private Vector3 _sunStartRotation;
        [SerializeField] private Vector3 _sunEndRotation;
        
        [Header("===== SUN INTENSITY SETTINGS =====")]
        [SerializeField] private AnimationCurve _intensityCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
        
        [Header("===== SUN INTENSITY SETTINGS =====")]
        [SerializeField] private AnimationCurve _temperatureCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
        
        private Light _sunLight;

        private void Awake()
        {
            _sunLight = GetComponent<Light>();
        }

        protected override void TimeBehavior()
        {
            Vector3 targetRotation = Vector3.Lerp(_sunStartRotation, _sunEndRotation, _state);
            transform.rotation = Quaternion.Euler(targetRotation);

            if (_sunLight)
            {
                _sunLight.intensity = _intensityCurve.Evaluate(_state);
                _sunLight.colorTemperature = _temperatureCurve.Evaluate(_state);
            }
        }
    }
}