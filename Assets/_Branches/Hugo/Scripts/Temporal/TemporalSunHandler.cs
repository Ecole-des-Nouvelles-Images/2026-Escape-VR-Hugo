using UnityEngine;

namespace _Branches.Hugo.Scripts.Temporal
{
    public class TemporalSunHandler : TemporalGameObject
    {
        [Header("===== SUN ROTATION SETTINGS =====")]
        [SerializeField] private Vector3 _sunStartRotation;
        [SerializeField] private Vector3 _sunEndRotation;
        
        protected override void TimeBehavior()
        {
            Vector3 targetRotation = Vector3.Lerp(_sunStartRotation, _sunEndRotation, _state);
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}