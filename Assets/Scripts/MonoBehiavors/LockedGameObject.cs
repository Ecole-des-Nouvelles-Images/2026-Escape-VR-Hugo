using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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

        [FormerlySerializedAs("_lockGameObjects")]
        [Header("===== REFERENCES =====")]
        [SerializeField] private List<GameObject> _lockedGameObjects = new();

        private List<LockedObjectCache> _cachedObjects = new();

        private void Start()
        {
            foreach (var obj in _lockedGameObjects)
            {
                if (obj == null) continue;

                LockedObjectCache cache = new LockedObjectCache
                {
                    Grab = obj.GetComponent<XRGrabInteractable>(),
                    Rb = obj.GetComponent<Rigidbody>()
                };

                if (cache.Grab) cache.Grab.enabled = false;
                if (cache.Rb) cache.Rb.constraints = RigidbodyConstraints.FreezeAll;

                _cachedObjects.Add(cache);
            }
        }

        public void UnlockGameObjects()
        {
            foreach (var cached in _cachedObjects)
            {
                if (cached.Grab) cached.Grab.enabled = true;
                if (cached.Rb) cached.Rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}