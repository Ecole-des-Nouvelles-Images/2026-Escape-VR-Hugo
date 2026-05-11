using UnityEngine;

namespace PuzzleLight
{
    public class PuzzleLightDoorHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MechanismLightReceiver _mechanismLightReceiver;
        [SerializeField] private Transform _doorTransform;
    
        [Header("Settings")]
        [SerializeField] private float _openAngle = 110f;
        [SerializeField] private float _openSpeed = 2f;

        [SerializeField] private Vector3 _pivotAxis;

        private Quaternion _closedRotation;
        private Quaternion _targetRotation;
        private bool _isOpened = false;

        private void Start()
        {
            _closedRotation = _doorTransform.localRotation;
            _targetRotation = _closedRotation;
        }

        void Update()
        {
            _doorTransform.localRotation = Quaternion.Slerp(_doorTransform.localRotation, _targetRotation, Time.deltaTime * _openSpeed);
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            if (_mechanismLightReceiver != null)
                _mechanismLightReceiver.OnLit += OpenDoor;
        }

        private void OnDisable()
        {
            if (_mechanismLightReceiver != null)
                _mechanismLightReceiver.OnLit -= OpenDoor;
        }
        
        private void OpenDoor()
        {
            if (_isOpened) return;
            _isOpened = true;
            _targetRotation = _closedRotation * Quaternion.Euler(_pivotAxis * _openAngle);
        }

        #endregion
    }
}
