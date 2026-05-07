using System;
using UnityEngine;

public class CollideFolowCamera : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _offSetPosition;
    
    private void Update()
    {
        Vector3 position = new Vector3(_target.transform.position.x, _target.transform.position.y + _offSetPosition.y, _target.transform.position.z);
        transform.position = position;
    }
}
