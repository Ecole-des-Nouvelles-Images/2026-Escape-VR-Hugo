using Core;
using UnityEngine;

namespace MonoBehiavors
{
    public class TriggerNarrationEvent04 : MonoBehaviour
    {
        public void StartNarrationEvent03()
        {
            EventBus.OnNarrationEvent04?.Invoke();
        }
    }
}