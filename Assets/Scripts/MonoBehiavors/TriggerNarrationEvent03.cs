using Core;
using UnityEngine;

namespace MonoBehiavors
{
    public class TriggerNarrationEvent03 : MonoBehaviour
    {
        public void StartNarrationEvent03()
        {
            EventBus.OnNarrationEvent03?.Invoke();
        }
    }
}