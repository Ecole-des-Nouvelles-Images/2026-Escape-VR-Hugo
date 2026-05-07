using System;
using UnityEngine;
using UnityEngine.Events;

public class WickHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flame"))
        {
            _event?.Invoke();
        }
    }
}
