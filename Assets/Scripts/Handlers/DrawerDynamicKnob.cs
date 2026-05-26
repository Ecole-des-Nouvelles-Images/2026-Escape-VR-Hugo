using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Handlers
{
    public class DrawerDynamicKnob : MonoBehaviour
    {
        [Header("===== SETTINGS =====")]
        [SerializeField] private Transform _targetRotation; 
        [SerializeField] private float _rotationSpeed = 10f;
        
        [Header("===== REFERENCES =====")]
        [SerializeField] private XRGrabInteractable _grabInteractable;
        
        [Header("===== DEBUG =====")]
        [SerializeField] private Transform _handTransform;
        [SerializeField] private Vector3 _offset;

        #region ===== EVENTS =====

        private void OnEnable()
        {
            if (!_grabInteractable) return;
            
            _grabInteractable.selectEntered.AddListener(OnSelectEntered);
            _grabInteractable.selectExited.AddListener(OnSelectExited);
        }
        
        private void OnDisable()
        {
            if (!_grabInteractable) return;
            
            _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            _grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

        private void OnSelectEntered(SelectEnterEventArgs context)
        {
            _handTransform = context.interactorObject.transform;
        }
        
        private void OnSelectExited(SelectExitEventArgs context)
        {
            _handTransform = null;
        }

        #endregion

        void Update()
        {
            if (!_targetRotation) return;

            float targetX = 0f;

            if (_handTransform)
            {
                Vector3 worldDirection = _handTransform.position + _offset - _targetRotation.position;
                Vector3 localDir = _targetRotation.parent.InverseTransformDirection(worldDirection);
                
                targetX = -Mathf.Atan2(localDir.z, -localDir.y) * Mathf.Rad2Deg;
            }

            Quaternion targetQuat = Quaternion.Euler(targetX, 0f, 0f);

            _targetRotation.localRotation = Quaternion.Slerp(
                _targetRotation.localRotation, 
                targetQuat, 
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}