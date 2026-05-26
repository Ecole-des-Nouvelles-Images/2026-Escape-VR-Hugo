using System.Collections;
using System.Collections.Generic;
using Core.Singletons;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR.Features.Meta;

namespace Managers
{
    public class FpsManager : MonoBehaviourSingletonDontDestroyOnLoad<FpsManager>
    {
        [Header("===== SET FPS =====")]
        [SerializeField] private int _targetFps = 72;
        
        // [Header("===== OPTIMIZATIONS =====")]
        // [SerializeField] private UniversalRenderPipelineAsset _renderPipelineAsset;
        // [SerializeField] private float _tickRate = 0.1f;
        //
        // private int _intervalFrameCount;
        // private float _currentTime;
        
        public void SetRefreshRate(int fps)
        {
            _targetFps = fps;
            StartCoroutine(SetRefreshRateCoroutine());
        }
        
        private void Start()
        {
            StartCoroutine(SetRefreshRateCoroutine());
        }

        // private void Update()
        // {
        //     _intervalFrameCount++;
        //     _currentTime += Time.deltaTime;
        //     
        //     if (_currentTime >= _tickRate)
        //     {
        //         float percentage = Mathf.InverseLerp(0f, _targetFps, 1 / Time.deltaTime);
        //         
        //         float value = Mathf.Lerp(0f, 3f, percentage);
        //         _renderPipelineAsset.renderScale = value;
        //         Debug.Log(value);
        //         
        //         _currentTime = 0f;
        //     }
        // }

        private IEnumerator SetRefreshRateCoroutine()
        {
            yield return null;
            
            List<XRDisplaySubsystem> displays = new();
            SubsystemManager.GetSubsystems(displays);
            
            foreach (var display in displays)
            {
                if (display == null || !display.running) continue;
                
                bool success = display.TryRequestDisplayRefreshRate(_targetFps);
                if (success) Time.fixedDeltaTime = 1f / _targetFps;
            }
        }
    }
}