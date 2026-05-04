using Core.Interfaces;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MobileStatueHandler : MonoBehaviour, ILightReactive
{
    [SerializeField] private LineRenderer _lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsLit()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out var hit))
        {
            if (hit.collider.TryGetComponent<ILightReactive>(out var lightReactive))
            {
                lightReactive.IsLit();
            }
           // UpdateLineRenderer()
        }
    }
}
