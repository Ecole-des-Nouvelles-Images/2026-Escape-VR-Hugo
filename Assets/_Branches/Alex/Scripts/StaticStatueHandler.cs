using _Branches.Hugo.Scripts;
using _Branches.Hugo.Scripts.Temporal;
using Core.Interfaces;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class StaticStatueHandler : TemporalGameObject
{
    [SerializeField] private LineRenderer _lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void TimeBehavior()
    {
        // if state > 0
        if (_state > 0)
        {
            // Active RayCast and line renderer
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<ILightReactive>(out var lightReactive))
                {
                    lightReactive.IsLit();
                }
               // UpdateLineRenderer(rayOrigin, hit.point);
            }
        }
    }
}
