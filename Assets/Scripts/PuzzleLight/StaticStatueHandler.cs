using Core.Audio;
using Core.Interfaces;
using FMODUnity;
using MonoBehiavors;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace PuzzleLight
{
    public class StaticStatueHandler : TemporalGameObject
    {
        [Header("===== SETTINGS =====")]
        [SerializeField] private LayerMask _layersToHit;
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private GameObject _splineExtrude;
        
        [Header("===== VISUAL LIGHT =====")]
        [SerializeField] private Vector3 _rayOffset;
        [SerializeField] private MeshRenderer _splineMeshRenderer;
        [SerializeField] private Transform _splineSparkle;
        [SerializeField] private float _sparkleOffset = -0.1f;

        [Header("===== FMOD AUDIO =====")] 
        [SerializeField] private EventReference _beamIgniteSFX;

        private ILightReactive _currentLitObject;
        
        private Spline _spline;
        private bool _isLit;
        private bool _wasLitLastFrame;
        
        private Material _material;

        private void Awake()
        {
            _material = _splineMeshRenderer.material;
        }

        void Start()
        {
            if (_splineContainer) _spline = _splineContainer.Spline;
            if (_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(false);
        }

        void Update()
        {
            if (_isLit && !_wasLitLastFrame)
            {
                if (AudioManager.Instance && !_beamIgniteSFX.IsNull)
                {
                    AudioManager.Instance.PlayAtPosition(_beamIgniteSFX, transform.position, loop: false);
                }
            }           
            
            _wasLitLastFrame = _isLit;
            
            if (_isLit)
            {
                ExecuteBeam();
            }
            else
            {
                StopBeam();
            }
        }

        protected override void TimeBehavior()
        {
            _isLit = _state > 0; 
        }

        private void ExecuteBeam()
        {
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward + _rayOffset;
        
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, _layersToHit))
            {
                if (hit.collider.TryGetComponent<ILightReactive>(out var lightReactive))
                {
                    if (hit.transform == transform) return;
                    
                    if (_currentLitObject == null)
                    {
                        lightReactive.OnLightEnter();
                        _currentLitObject = lightReactive;
                    }
                    else if (_currentLitObject != lightReactive)
                    {
                        _currentLitObject.OnLightExit();
                        lightReactive.OnLightEnter();
                        _currentLitObject = lightReactive;
                    }
                }
                else
                {
                    if (_currentLitObject != null)
                    {
                        _currentLitObject.OnLightExit();
                        _currentLitObject = null;
                    }
                }
                
                UpdateLineRenderer(rayOrigin, hit.point, hit.normal);
            }
        }

        private void StopBeam()
        {
            if (_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(false);
        }

        private void UpdateLineRenderer(Vector3 start, Vector3 end, Vector3 hitNormal)
        {
            if (!_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(true);

            float3 localStart = transform.InverseTransformPoint(start);
            float3 localEnd = transform.InverseTransformPoint(end);

            _spline.SetKnot(0, new BezierKnot(localStart));
            _spline.SetKnot(1, new BezierKnot(localEnd));
            
            // SHADER
            _material.SetFloat("_SplineLength", Vector3.Distance(localStart, localEnd) / 2);
            
            // SPARKLE
            if (_splineSparkle != null)
            {
                var vector3 = _splineSparkle.localPosition;
                vector3.z = localEnd.z + _sparkleOffset;
                _splineSparkle.localPosition = vector3;
                
                _splineSparkle.rotation = Quaternion.LookRotation(-hitNormal);
            }
        }
    }
}
