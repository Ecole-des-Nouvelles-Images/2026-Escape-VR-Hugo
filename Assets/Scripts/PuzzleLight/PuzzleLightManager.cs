using Core;
using UnityEngine;

namespace PuzzleLight
{
    public class PuzzleLightManager : MonoBehaviour
    {
        [Header("===== DEBUGS =====")]
        [SerializeField] private bool _isStaticStatuetteEnlightened;
        [SerializeField] private bool _isSecondStatuetteEnlightened;
        [SerializeField] private bool _isMechanismUnlocked;
        [SerializeField] private bool _isFirstElementActivated;
        [SerializeField] private bool _isSecondElementActivated;
        [SerializeField] private bool _isThirdElementActivated;
        [SerializeField] private bool _isLightKeyUnlocked;
        
        #region ===== EVENTS =====

        private void OnEnable()
        {
            EventBus.OnStaticStatuetteEnlightened += OnStaticStatuetteEnlightened;
            EventBus.OnSecondStatuetteEnlightened += OnSecondStatuetteEnlightened;
            EventBus.OnMechanismUnlocked += OnMechanismUnlocked;
            EventBus.OnFirstElementActivated += OnFirstElementActivated;
            EventBus.OnSecondElementActivated += OnSecondElementActivated;
            EventBus.OnThirdElementActivated += OnThirdElementActivated;
            EventBus.OnLightKeyUnlocked += OnLightKeyUnlocked;
        }
        
        private void OnDisable()
        {
            EventBus.OnStaticStatuetteEnlightened -= OnStaticStatuetteEnlightened;
            EventBus.OnSecondStatuetteEnlightened -= OnSecondStatuetteEnlightened;
            EventBus.OnMechanismUnlocked -= OnMechanismUnlocked;
            EventBus.OnFirstElementActivated -= OnFirstElementActivated;
            EventBus.OnSecondElementActivated -= OnSecondElementActivated;
            EventBus.OnThirdElementActivated += OnThirdElementActivated;
            EventBus.OnLightKeyUnlocked += OnLightKeyUnlocked;
        }

        private void OnStaticStatuetteEnlightened()
        {
            _isStaticStatuetteEnlightened = true;
        }

        private void OnSecondStatuetteEnlightened()
        {
            _isSecondStatuetteEnlightened = true;
        }

        private void OnMechanismUnlocked()
        {
            _isMechanismUnlocked = true;
        }

        private void OnFirstElementActivated()
        {
            _isFirstElementActivated = true;
        }

        private void OnSecondElementActivated()
        {
            _isSecondElementActivated = true;
        }
        
        private void OnThirdElementActivated()
        {
            _isThirdElementActivated = true;
        }
        
        private void OnLightKeyUnlocked()
        {
            _isLightKeyUnlocked = true;
        }

        #endregion
    }
}