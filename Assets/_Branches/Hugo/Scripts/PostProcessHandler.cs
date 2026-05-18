using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Branches.Hugo.Scripts
{
    public class PostProcessHandler : MonoBehaviour
    {
        [Header("===== RENDERER FEATURE =====")]
        [SerializeField] private UniversalRendererData _rendererData;
        [SerializeField] private string _featureName = "FeatureName";
        
        [Header("===== UNIVERSAL RENDER PIPELINE =====")]
        [SerializeField] private UniversalRenderPipelineAsset _renderPipelineAsset;

        [Header("===== VOLUME EFFECTS =====")]
        [SerializeField] private Volume _globalVolume;

        private ChromaticAberration _chromaticAberration;
        private ScriptableRendererFeature _targetFeature;

        void Awake()
        {
            if (_rendererData != null)
            {
                _targetFeature = _rendererData.rendererFeatures.Find(f => f.name == _featureName);
                if (_targetFeature == null)
                {
                    Debug.LogWarning($"[GraphicsController] Feature '{_featureName}' introuvable !");
                }
            }

            if (_globalVolume != null && _globalVolume.profile != null)
            {
                // On essaie de récupérer le composant existant dans le profil du Volume
                if (!_globalVolume.profile.TryGet(out _chromaticAberration))
                {
                    Debug.LogWarning("[GraphicsController] Chromatic Aberration manquante dans le profil du Volume.");
                }
            }
        }

        /// <summary>
        /// Active ou désactive la Renderer Feature
        /// </summary>
        public void SetRendererFeatureActive(bool active)
        {
            if (_targetFeature == null) return;
            _targetFeature.SetActive(active);
            _rendererData.SetDirty(); 
            
            if (_renderPipelineAsset) _renderPipelineAsset.supportsCameraOpaqueTexture = active;
        }

        /// <summary>
        /// Active ou désactive l'effet d'aberration chromatique
        /// </summary>
        public void SetChromaticAberrationActive(bool active)
        {
            if (_chromaticAberration == null) return;
            _chromaticAberration.active = active;
        }
    }
}