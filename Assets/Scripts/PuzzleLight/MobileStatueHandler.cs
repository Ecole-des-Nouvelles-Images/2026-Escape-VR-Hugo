using Core.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace PuzzleLight
{
    public class MobileStatueHandler : MonoBehaviour, ILightReactive
    {
        [Header("===== SETTINGS =====")]
        [SerializeField, Tooltip("Time in seconds for the statue to stay lit after being hit by a raycast.")] private float _litDuration = 0.08f;
        [SerializeField] private LayerMask _layersToHit;
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private GameObject _splineExtrude;
        
        private Spline _spline;
        private float _lastLitTime = -Mathf.Infinity;

        void Start()
        {
            if (_splineContainer) _spline = _splineContainer.Spline;
            if (_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(false);
        }

        void LateUpdate()
        {
            if (Time.time - _lastLitTime > _litDuration)
            {
                if (_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(false);
            }
        }

        public void IsLit()
        {
            _lastLitTime = Time.time;
        
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            if (Physics.Raycast(origin, direction, out var hit, Mathf.Infinity, _layersToHit))
            {
                if (hit.collider.TryGetComponent<ILightReactive>(out var lightReactive))
                {
                    if (hit.transform != transform)
                        lightReactive.IsLit();
                }
                UpdateLineRenderer(origin, hit.point);
            }
            else
            {
                if (_splineExtrude.activeInHierarchy) _splineExtrude.SetActive(false);
            }
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
