using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Player
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class GrabAutoDrop : MonoBehaviour
    {
        private XRGrabInteractable _grabInteractable;
        private ClockTimeManager _clockTimeManager;

        private void Awake()
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void Start()
        {
            _clockTimeManager = ClockTimeManager.Instance;
        }

        private void Update()
        {
            if (!_grabInteractable.isSelected) return;
            
            if (Mathf.Approximately(_clockTimeManager.NormalizedCurrentTime, 1f)
                || Mathf.Approximately(_clockTimeManager.NormalizedCurrentTime, 0f))
            {
                TriggerAutoDrop();
            }
        }

        /// <summary>
        /// Force la désélection de l'objet de manière atomique et propre.
        /// </summary>
        private void TriggerAutoDrop()
        {
            _grabInteractable.interactionManager.SelectExit(_grabInteractable.firstInteractorSelecting, _grabInteractable);
        }
    }
}