using _Branches.Hugo.Scripts;
using Core.Interfaces;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class StaticStatueHandler : TemporalGameObject
{
    [SerializeField] private LineRenderer _lineRenderer;
    
    //TODO Call raycast and linerenderer outside TimeBehaviour()
    
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
        if (_state > 0)
        {
            // Active RayCast and line renderer
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;
            
            Debug.DrawRay(rayOrigin, rayDirection, Color.green);

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<ILightReactive>(out var lightReactive))
                {
                    lightReactive.IsLit();
                }
                UpdateLineRenderer(rayOrigin, hit.point);
            }
        }
    }

    private void UpdateLineRenderer(Vector3 start, Vector3 end)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
