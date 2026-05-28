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
            if (!IsLock) return;
            IsLock = false;
            UnityEvent?.Invoke();
            
        }
    }
}
