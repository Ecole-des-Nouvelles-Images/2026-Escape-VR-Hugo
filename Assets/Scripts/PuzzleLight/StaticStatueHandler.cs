using _Branches.Hugo.Scripts;
using Core.Interfaces;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class StaticStatueHandler : TemporalGameObject
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LayerMask _layersToHit;
    
    private bool _isBeamActive = false;
    
    //TODO avoid raycast stopping on socket collider
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
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
        // Active RayCast and line renderer
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
        if (_lineRenderer.positionCount != 0)
            _lineRenderer.positionCount = 0;
    }

    private void UpdateLineRenderer(Vector3 start, Vector3 end)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
