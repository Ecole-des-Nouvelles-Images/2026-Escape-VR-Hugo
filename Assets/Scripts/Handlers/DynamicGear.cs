using System;
using System.Collections.Generic;
using DG.Tweening;
using MonoBehiavors;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Handlers
{
        public class DynamicGear : MonoBehaviour
    {
        // CORRECTION : L'événement transmet désormais : <IndexDeL_Engrenage, ValeurDuChiffre>
        public Action<int, int> CodeChanged;
        
        [Header("===== SETTINGS =====")]
        [Tooltip("Index unique de cet engrenage (0 pour le 1er, 1 pour le 2e, etc.)")]
        [SerializeField] private int _gearIndex; 
        [SerializeField] private Transform _targetRotation; 
        [SerializeField] private float _rotationSpeed = 25f;
        
        [Header("===== REFERENCES =====")]
        [SerializeField] private XRSimpleInteractable _interactable;
        [SerializeField] private TriggerCollider _trigger;
        
        [Header("===== ANIMATION =====")]
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private AnimationCurve _animationCurve;
        
        [Header("===== DEBUG (AUTOMATIC) =====")]
        [SerializeField] private Transform _handTransform;
        private Vector3 _offset; 
    
        private float _lastHandAngle; 
        private float _currentSmoothX; 
        private Tween _snapTween;
    
        private void Start()
        {
            if (_targetRotation)
            {
                _currentSmoothX = _targetRotation.localRotation.eulerAngles.x;
            }
        }
    
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
        
            // Recalcul de l'angle de l'engrenage pour être sûr de partir d'une valeur propre
            float rawAngle = _targetRotation.localRotation.eulerAngles.x;
            _currentSmoothX = _currentSmoothX + Mathf.DeltaAngle(_currentSmoothX, rawAngle);
            
            // L'angle de référence initial est l'angle actuel de la main
            _lastHandAngle = CalculateCurrentHandAngle();
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
        
            // 1. On récupère l'angle de la main à CETTE frame
            float currentHandAngle = CalculateCurrentHandAngle();
        
            // 2. On calcule la différence entre la frame PRÉCÉDENTE et MAINTENANT
            float frameAngleDelta = Mathf.DeltaAngle(_lastHandAngle, currentHandAngle);
        
            // 3. On applique directement ce petit changement à notre cible
            float targetX = _currentSmoothX + frameAngleDelta;
        
            // 4. On lisse le mouvement pour l'effet physique
            _currentSmoothX = Mathf.Lerp(_currentSmoothX, targetX, _rotationSpeed * Time.deltaTime);
        
            // 5. On applique la rotation finale
            _targetRotation.localRotation = Quaternion.Euler(_currentSmoothX, 0f, 0f);
        
            // CRUCIAL : On sauvegarde l'angle actuel pour qu'il devienne l'angle "précédent" à la prochaine frame
            _lastHandAngle = currentHandAngle;
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
    
            Quaternion targetQuat = Quaternion.Euler(snappedX, currentRotation.y, currentRotation.z);
            
            _snapTween = targetTransform.DOLocalRotateQuaternion(targetQuat, _duration).SetEase(_animationCurve)
                .OnComplete(() =>
                {
                    _currentSmoothX = snappedX;
    
                    List<GameObject> go = _trigger.GetGameObjectsWithTag("PadlockValue");
                    if (go != null && go.Count > 0)
                    {
                        int currentDigit = Convert.ToInt32(go[0].name);
                        
                        // CORRECTION : On envoie l'ID d'index immuable configuré dans l'inspecteur
                        CodeChanged?.Invoke(_gearIndex, currentDigit);
                    }
                });
        }
    }
}