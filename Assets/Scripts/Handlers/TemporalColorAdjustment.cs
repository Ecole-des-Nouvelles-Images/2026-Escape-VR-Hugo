using MonoBehiavors;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Handlers
{
    public class TemporalColorAdjustment : TemporalGameObject
    {
        [Header("===== VOLUME EFFECTS =====")]
        [SerializeField] private Volume _globalVolume;
        
        [Header("===== EXPOSURE SETTINGS =====")]
        [SerializeField] private AnimationCurve _expCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
        
        [Header("===== COLOR FILTER SETTINGS =====")]
        [SerializeField] private Gradient _colorFilterGradient;
        
        private ColorAdjustments _colorAdjustments;

        private void Awake()
        {
            // IMPORTANT : On utilise sharedProfile pour inspecter le profil réel assigné au Volume
            if (_globalVolume != null && _globalVolume.sharedProfile != null)
            {
                if (!_globalVolume.sharedProfile.TryGet(out _colorAdjustments))
                {
                    Debug.LogWarning("[TemporalColorAdjustment] Color Adjustments manquant dans le profil du Volume.");
                }
            }
        }

        protected override void TimeBehavior()
        {
            if (!_colorAdjustments) return;
            
            _colorAdjustments.postExposure.value = _expCurve.Evaluate(_state);
            _colorAdjustments.colorFilter.value = _colorFilterGradient.Evaluate(_state);
        }
    }
}