using Core.Interfaces;
using UnityEngine;

namespace PuzzleLight
{
    [RequireComponent(typeof(LineRenderer))]
    public class MobileStatueHandler : MonoBehaviour, ILightReactive
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private LayerMask _layersToHit;
        [SerializeField, Tooltip("Time in seconds for the statue to stay lit after being hit by a raycast.")] private float _litDuration = 0.08f;
    
        private float _lastLitTime = -Mathf.Infinity;

        void Start()
        {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
        }

        void LateUpdate()
        {
            if (Time.time - _lastLitTime > _litDuration)
            {
                _lineRenderer.positionCount = 0;
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
                _lineRenderer.positionCount = 0;
            }
        }

        private void UpdateLineRenderer(Vector3 start, Vector3 end)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }
    }
}
