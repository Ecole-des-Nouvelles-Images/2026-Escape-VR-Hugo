using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Player
{
    public class HandAutoDrop : MonoBehaviour
    {
        [Header("===== SETTINGS =====")]
        [Tooltip("Distance maximale (en mètres) autorisée entre la main et l'objet avant de lâcher.")]
        [SerializeField] private float _maxGrabDistance = 0.5f;

        private XRBaseInteractor _interactor;

        private void Awake()
        {
            _interactor = GetComponent<XRBaseInteractor>();
        }

        private void Update()
        {
            if (!_interactor.hasSelection) return;

            IXRSelectInteractable targetInteractable = _interactor.firstInteractableSelected;
            if (targetInteractable == null) return;

            Transform interactableTransform = targetInteractable.transform;

            float currentDistance = Vector3.Distance(transform.position, interactableTransform.position);

            if (currentDistance > _maxGrabDistance)
            {
                TriggerAutoDrop(targetInteractable);
            }
        }

        /// <summary>
        /// Force la main à lâcher l'objet de manière propre et atomique.
        /// </summary>
        private void TriggerAutoDrop(IXRSelectInteractable interactable)
        {
            _interactor.interactionManager.SelectExit(_interactor, interactable);
        }

        #region ===== DEBUG GIZMOS =====

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(transform.position, _maxGrabDistance);
        }

        #endregion
    }
}