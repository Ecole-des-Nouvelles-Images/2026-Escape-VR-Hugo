using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipableObject : MonoBehaviour
{
    public bool CanBeEquipable;
    
    private List<BoxCollider> _colliders;
    private Rigidbody _rb; 
    
    private void Awake()
    {
        _colliders = GetComponentsInChildren<BoxCollider>().ToList();
        _rb = GetComponent<Rigidbody>();
    }

    [ContextMenu("Enable")]
    public void EnableCollider()
    {
        foreach (BoxCollider col in _colliders)
        {
            //col.isTrigger = false;
            _rb.isKinematic = false;
        }
    }
    
    [ContextMenu("Disable")]
    public void DisableCollider()
    {
        foreach (BoxCollider col in _colliders)
        {
           //col.isTrigger = true;
            _rb.isKinematic = true;
        }
    }
}
