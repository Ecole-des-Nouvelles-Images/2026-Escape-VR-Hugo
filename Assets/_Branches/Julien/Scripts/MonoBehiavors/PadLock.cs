using UnityEngine;
using UnityEngine.Events;

public class PadLock : MonoBehaviour
{
    public bool IsLock = true;
    public UnityEvent UnityEvent;
    
    protected virtual void UnlockPadLock()
    {
        IsLock = false;
        UnityEvent?.Invoke();
        Debug.Log("UnlockPadLock");
    }
}
