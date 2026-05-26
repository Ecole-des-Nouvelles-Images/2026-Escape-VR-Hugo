using UnityEngine;
using UnityEngine.Events;

namespace MonoBehiavors
{
    public class PadLock : MonoBehaviour
    {
        [Header("===== GLOBAL SETTINGS =====")]
        public bool IsLock = true;
        public UnityEvent UnityEvent;
    
        protected virtual void UnlockPadLock()
        {
            IsLock = false;
            UnityEvent?.Invoke();
            Debug.Log("UnlockPadLock");
        }
    }
}
