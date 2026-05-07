using System;
using Core.Interfaces;
using UnityEngine;

public class PuzzleLightDoorHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LightReceiver _lightReceiver;
    [SerializeField] private Transform _doorTransform;
    
    [Header("Settings")]
    [SerializeField] private float _openAngle = 110f;
    [SerializeField] private float _openSpeed = 2f;

    [SerializeField] private Vector3 _pivotAxis;

    private Quaternion _closedRotation;
    private Quaternion _targetRotation;
    private bool _isOpened = false;

    private void Start()
    {
        _closedRotation = _doorTransform.localRotation;
        _targetRotation = _closedRotation;

        if (_lightReceiver != null)
            _lightReceiver.OnLit += OpenDoor;
    }

    // Update is called once per frame
    void Update()
    {
        _doorTransform.localRotation = Quaternion.Slerp(_doorTransform.localRotation, _targetRotation, Time.deltaTime * _openSpeed);
    }


    private void OpenDoor()
    {
        if (_isOpened) return;
        _isOpened = true;
        _targetRotation = _closedRotation * Quaternion.Euler(_pivotAxis * _openAngle);
    }

    private void OnDestroy()
    {
        if (_lightReceiver != null)
            _lightReceiver.OnLit -= OpenDoor;
    }
}
