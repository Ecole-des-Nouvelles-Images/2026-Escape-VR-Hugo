using Core.Interfaces;
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
    
        private Spline _spline;
        private bool _isBeamActive = false;
        
        void Start()
        {
            if (_splineContainer) _spline = _splineContainer.Spline;
            if (_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(false);
        }

        void Update()
        {
            if (_isBeamActive)
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
            _isBeamActive = _state > 0; 
        }

        private void ExecuteBeam()
        {
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;
        
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, _layersToHit))
            {
                if (hit.collider.TryGetComponent<ILightReactive>(out var lightReactive))
                {
                    lightReactive.IsLit();
                }
                UpdateLineRenderer(rayOrigin, hit.point);
            }
        }

        private void StopBeam()
        {
            if (_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(false);
        }

        private void UpdateLineRenderer(Vector3 start, Vector3 end)
        {
            if (!_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(true);

            float3 localStart = transform.InverseTransformPoint(start);
            float3 localEnd = transform.InverseTransformPoint(end);

            _spline.SetKnot(0, new BezierKnot(localStart));
            _spline.SetKnot(1, new BezierKnot(localEnd));
        }
    }
}
