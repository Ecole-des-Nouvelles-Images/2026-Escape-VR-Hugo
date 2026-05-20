using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Handlers
{
    public class DynamicGear : MonoBehaviour
    {
        public Action<int> CodeChanged;
        
        [Header("===== SETTINGS =====")]
        [SerializeField] private Transform _targetRotation; 
        [SerializeField] private float _rotationSpeed = 15f;
        
        [Header("===== REFERENCES =====")]
        [SerializeField] private XRSimpleInteractable _interactable;
        
        [Header("===== ANIMATION =====")]
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private AnimationCurve _animationCurve;
        
        [Header("===== DEBUG =====")]
        [SerializeField] private Transform _handTransform;
        [SerializeField] private Vector3 _offset;

        private float _initialHandAngle;
        private float _initialGearAngleX;
        private Tween _snapTween;

        #region ===== EVENTS =====

        private void OnEnable()
        {
            if (!_interactable) return;
            
            _interactable.selectEntered.AddListener(OnSelectEntered);
            _interactable.selectExited.AddListener(OnSelectExited);
        }
        
        private void OnDisable()
        {
            if (!_interactable) return;
            
            _interactable.selectEntered.RemoveListener(OnSelectEntered);
            _interactable.selectExited.RemoveListener(OnSelectExited);
        }

        private void OnSelectEntered(SelectEnterEventArgs context)
        {
            if (_snapTween != null && _snapTween.IsActive()) _snapTween.Kill();

            _handTransform = context.interactorObject.transform;

            _initialGearAngleX = _targetRotation.localRotation.eulerAngles.x;
            _initialHandAngle = CalculateCurrentHandAngle();
        }
        
        private void OnSelectExited(SelectExitEventArgs context)
        {
            _handTransform = null;
            SnapOnlyXAxis(_targetRotation);
        }

        #endregion

        void Update()
        {
            if (!_targetRotation || !_handTransform) return;

            float currentHandAngle = CalculateCurrentHandAngle();

            float angleDelta = Mathf.DeltaAngle(_initialHandAngle, currentHandAngle);

            float targetX = _initialGearAngleX + angleDelta;
            Quaternion targetQuat = Quaternion.Euler(targetX, 0f, 0f);

            _targetRotation.localRotation = Quaternion.Slerp(
                _targetRotation.localRotation, 
                targetQuat, 
                _rotationSpeed * Time.deltaTime
            );
        }

        private float CalculateCurrentHandAngle()
        {
            Vector3 worldDirection = _handTransform.position + _offset - _targetRotation.position;
            Vector3 localDir = _targetRotation.parent.InverseTransformDirection(worldDirection);
            return -(Mathf.Atan2(localDir.z, -localDir.y) * Mathf.Rad2Deg);
        }

        private void SnapOnlyXAxis(Transform targetTransform)
        {
            if (targetTransform == null) return;

            Vector3 currentRotation = targetTransform.localRotation.eulerAngles;
            float snappedX = Mathf.Repeat(currentRotation.x, 360f);
            snappedX = Mathf.Round(snappedX / 36f) * 36f;

            if (snappedX > 360f) snappedX = 360f;

            // Index de 0 à 10
            int currentDigit = Mathf.RoundToInt(snappedX / 36f);

            Quaternion targetQuat = Quaternion.Euler(snappedX, currentRotation.y, currentRotation.z);
            
            _snapTween = targetTransform.DOLocalRotateQuaternion(targetQuat, _duration).SetEase(_animationCurve);
            
            Debug.Log($"[DynamicGear] Angle final : {snappedX}° -> Index envoyé : {currentDigit}");
            CodeChanged?.Invoke(currentDigit);
        }
    }
}