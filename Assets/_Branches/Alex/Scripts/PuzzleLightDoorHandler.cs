using System;
using Core.Interfaces;
using UnityEngine;

public class PuzzleLightDoorHandler : MonoBehaviour, ILightReactive
{
    [SerializeField] private float _openAngle = 110f;
    [SerializeField] private float _openSpeed = 2f;

    [SerializeField] private Vector3 _pivotAxis;

    private Quaternion _closedRotation;
    private Quaternion _targetRotation;
    private bool _isOpening = false;

    
    //TODO Separate Door and Light Collider
    private void Awake()
    {
        _closedRotation = transform.localRotation;
        _targetRotation = _closedRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, Time.deltaTime * _openSpeed);
    }

    public void IsLit()
    {
        if (!_isOpening)
        {
            _isOpening = true;
            _targetRotation = _closedRotation * Quaternion.Euler(_pivotAxis * _openAngle);
        }
    }
}
