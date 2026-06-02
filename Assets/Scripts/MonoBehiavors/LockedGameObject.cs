using System.Collections;
using System.Collections.Generic;
using Core.Audio;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace MonoBehiavors
{
    public class LockedGameObject : MonoBehaviour
    {
        private struct LockedObjectCache
        {
            public XRGrabInteractable Grab;
            public Rigidbody Rb;
        }
        
        [Header("===== SETTINGS =====")]
        [SerializeField] private bool _isLocked = true;

        [Header("===== REFERENCES GAMEOBJECTS =====")]
        [SerializeField] private GameObject _context;
        [SerializeField] private List<GameObject> _insideGameObjects = new();
        
        [Header("==== TRIGGER =====")]
        [SerializeField] private TriggerCollider _insideTrigger;
        
        [Header("===== RAYCAST =====")]
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private float _raycastLength = 5f;
        [SerializeField] private LayerMask _raycastMask;
        
        [Header("===== FMOD AUDIO =====")]
        [SerializeField] private FMODUnity.EventReference _openSFX;
        [SerializeField] private FMODUnity.EventReference _closeSFX;

        [Header("===== DEBUG =====")]
        [SerializeField] private bool _isOpen;

        private LockedObjectCache _cachedContext;
        private GameObject _lastHitObject;

        private IEnumerator Start()
        {
            if (_raycastOrigin == null) _raycastOrigin = transform;

            if (_isLocked)
            {
                _cachedContext = FindComponentsEverywhere(_context);
            
                if (_cachedContext.Grab) _cachedContext.Grab.enabled = false;
                if (_cachedContext.Rb) _cachedContext.Rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            
            yield return new WaitForEndOfFrame();
            
            _insideGameObjects = _insideTrigger.GetGameObjects();
            if (_insideGameObjects.Count > 0)
            {
                foreach (var obj in _insideGameObjects)
                {
                    if (obj == null) continue;
                    ApplyLockState(obj, true);
                }
            }
        }

        private void Update()
        {
            ExecuteRaycastCheck();
        }

        #region ===== PUBLIC =====

        public void UnlockContext()
        {
            if (_cachedContext.Grab) _cachedContext.Grab.enabled = true;
            if (_cachedContext.Rb) _cachedContext.Rb.constraints = RigidbodyConstraints.None;
        }

        #endregion
        
        #region ===== PRIVATE =====
        
        private void ExecuteRaycastCheck()
        {
            Vector3 origin = _raycastOrigin.position;
            Vector3 direction = _raycastOrigin.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, _raycastLength, _raycastMask))
            {
                if (!_lastHitObject)
                {
                    GameObject currentHitObject = hit.collider.gameObject;
                    
                    LockGameObjects();
                    _lastHitObject = currentHitObject;

                    _isOpen = false;
                    
                    if (AudioManager.Instance && !_closeSFX.IsNull)
                    {
                        AudioManager.Instance.Play(_closeSFX, loop: false, follow: gameObject);
                    }
                }
            }
            else
            {
                if (_lastHitObject)
                {
                    UnlockGameObjects();
                    _lastHitObject = null;
                    
                    _isOpen = true;
                    
                    if (AudioManager.Instance && !_openSFX.IsNull)
                    {
                        AudioManager.Instance.Play(_openSFX, loop: false, follow: gameObject);
                    }
                }
            }
        }
        
        private void LockGameObjects()
        {
            _insideGameObjects = _insideTrigger.GetGameObjects();
            if (_insideGameObjects.Count > 0)
            {
                foreach (var obj in _insideGameObjects)
                {
                    if (!obj) continue;
                    ApplyLockState(obj, true);
                }
            }
        }
        
        private void UnlockGameObjects()
        {
            _insideGameObjects = _insideTrigger.GetGameObjects();
            if (_insideGameObjects.Count > 0)
            {
                foreach (var obj in _insideGameObjects)
                {
                    if (!obj) continue;
                    ApplyLockState(obj, false);
                }
            }
        }

        /// <summary>
        /// Applique l'état de verrouillage de manière sécurisée en cherchant partout dans la hiérarchie
        /// </summary>
        private void ApplyLockState(GameObject target, bool shouldLock)
        {
            LockedObjectCache cache = FindComponentsEverywhere(target);

            if (cache.Grab) 
            {
                cache.Grab.enabled = !shouldLock;
            }

            // if (cache.Rb) 
            // {
            //     cache.Rb.constraints = shouldLock ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
            // }
        }
        
        /// <summary>
        /// Méthode outil robuste qui va chercher les composants n'importe où dans la hiérarchie de l'objet
        /// </summary>
        private LockedObjectCache FindComponentsEverywhere(GameObject target)
        {
            LockedObjectCache cache = new LockedObjectCache();
            if (target == null) return cache;

            cache.Grab = target.GetComponent<XRGrabInteractable>();
            if (cache.Grab == null) cache.Grab = target.GetComponentInParent<XRGrabInteractable>();
            if (cache.Grab == null) cache.Grab = target.GetComponentInChildren<XRGrabInteractable>();

            // cache.Rb = target.GetComponent<Rigidbody>();
            // if (cache.Rb == null) cache.Rb = target.GetComponentInParent<Rigidbody>();
            // if (cache.Rb == null) cache.Rb = target.GetComponentInChildren<Rigidbody>();

            return cache;
        }
        
        #endregion
        
        #region ===== UNITY GIZMOS =====

        private void OnDrawGizmos()
        {
            Transform originTransform = _raycastOrigin != null ? _raycastOrigin : transform;
            Vector3 origin = originTransform.position;
            Vector3 direction = originTransform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, _raycastLength, _raycastMask))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin, hit.point);
                Gizmos.DrawWireSphere(hit.point, 0.02f);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(origin, origin + direction * _raycastLength);
            }
        }

        #endregion
    }
}