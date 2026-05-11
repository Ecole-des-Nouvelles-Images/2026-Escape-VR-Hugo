using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace _Branches.Hugo.Scripts.Player
{
    public class HandDistanceUnclasp : MonoBehaviour
    {
        [Header("===== CONFIGURATION =====")]
        [Tooltip("Distance maximum avant de lâcher l'objet (en mètres)")]
        [SerializeField] private float _maxDistance = 0.4f;
        
        [Header("===== REFERENCES =====")]
        [SerializeField] private XRDirectInteractor _interactor;

        private void Awake()
        {
            if (_interactor == null) _interactor = GetComponent<XRDirectInteractor>();
        }

        void Update()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            // On vérifie si la main tient quelque chose
            if (_interactor.hasSelection)
            {
                // On récupère l'objet actuellement tenu
                IXRSelectInteractable interactable = _interactor.firstInteractableSelected;
                
                if (interactable != null)
                {
                    // Calcul de la distance entre la main (interactor) et l'objet
                    float currentDistance = Vector3.Distance(_interactor.transform.position, interactable.transform.position);

                    if (currentDistance > _maxDistance)
                    {
                        ForceRelease(currentDistance);
                    }
                }
            }
        }

        private void ForceRelease(float currentDistance)
        {
            Debug.Log($"[VR] Distance trop grande ({currentDistance}m). Lâcher forcé !");
            
            // On demande au manager de terminer la sélection (ce qui force le lâcher)
            _interactor.interactionManager.SelectExit((IXRSelectInteractor)_interactor, _interactor.firstInteractableSelected);
        }

        // Visualisation de la limite dans l'éditeur
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _maxDistance);
        }
    }
}