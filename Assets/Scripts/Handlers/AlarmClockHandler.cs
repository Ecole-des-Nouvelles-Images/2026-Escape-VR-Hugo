using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Handlers
{
    public class AlarmClockHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private XRSocketInteractor _alarmMechanismSockets;
        [SerializeField] private XRSocketInteractor _alarmFaceSockets;
        
        [Header("===== DEBUG =====")]
        [SerializeField] private bool _isCompleted;
        [SerializeField] private int _advancement;

        public float GetAdvancement()
        {
            return _advancement;
        }

        public void OnMechanismPlaced(SelectEnterEventArgs args)
        {
            _advancement++;
            
            StartCoroutine(LockObjectInSocket(args.interactableObject.transform, _alarmMechanismSockets));
        }
        
        public void OnFacePlaced(SelectEnterEventArgs args)
        {
            _advancement++;
            _isCompleted = true;
            EventBus.OnAlarmRepaired?.Invoke();
            
            StartCoroutine(LockObjectInSocket(args.interactableObject.transform, _alarmFaceSockets));
        }

        private IEnumerator LockObjectInSocket(Transform go, XRSocketInteractor socket)
        {
            yield return new WaitForEndOfFrame();

            XRGrabInteractable grab = go.GetComponent<XRGrabInteractable>();
            Rigidbody rb = go.GetComponent<Rigidbody>();
            Collider coll = go.GetComponentInChildren<Collider>();

            if (grab) grab.enabled = false;
            
            if (rb)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
            }

            if (coll) coll.isTrigger = true;

            go.SetParent(socket.transform);
            go.localPosition = Vector3.zero;
            go.localRotation = Quaternion.identity;

            socket.enabled = false;
        }
    }
}