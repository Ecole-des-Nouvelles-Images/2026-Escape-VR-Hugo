using UnityEngine;
using UnityEngine.Events;

namespace Handlers
{
    public class WickHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent _event;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Flame"))
            {
                CandleHandler candlerHandler = other.gameObject.transform.parent.GetComponent<CandleHandler>();
                if (candlerHandler.IsFire)
                {
                    _event?.Invoke();
                }
            }
        }
    }
}
