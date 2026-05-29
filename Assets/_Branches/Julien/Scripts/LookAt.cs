using System;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private bool _doLookAt = true;
    
    [Header("Lock Value")] 
    [SerializeField] private float _valueX;
    [SerializeField] private float _valueY;
    [SerializeField] private float _valueZ;
    private void Update()
    {
        if (!_doLookAt) return;
        
        transform.LookAt(_target);

        Vector3 rotation = transform.eulerAngles;
        
        if (_valueX != 0){ rotation.x = _valueX;}
        if (_valueY != 0){ rotation.y = _valueY;}
        if (_valueZ != 0){ rotation.z = _valueZ;}

        transform.eulerAngles = rotation;
    }
}
