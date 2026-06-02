using System.Collections;
using Core;
using Core.Audio;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Handlers
{
    public class AlarmClockHandler : MonoBehaviour
    {
        [Header("===== REFERENCES SOCKETS =====")]
        [SerializeField] private XRSocketInteractor _alarmMechanismSockets;
        [SerializeField] private XRSocketInteractor _alarmFaceSockets;
        
        [Header("===== HOURS HANDS =====")]
        [SerializeField] private Transform _hoursHandTransform;
        [SerializeField] private Vector3 _hoursPivotAxis = Vector3.up;
        [SerializeField] private float _hoursAngle = 90f;
        
        [Header("===== MINUTES HANDS =====")]
        [SerializeField] private Transform _minutesHandTransform;
        [SerializeField] private Vector3 _minutesPivotAxis = Vector3.up;
        [SerializeField] private float _minutesAngle = 90f;
        
        [Header("===== ANIMATIONS =====")]
        [SerializeField] private float _duration = 1.5f;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("===== FMOD AUDIO =====")] 
        [SerializeField] private EventReference _gearInsertSFX;
        [SerializeField] private EventReference _alarmRepairedSFX;
        
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
            
            PlaySoundAtPosition(_gearInsertSFX, _alarmMechanismSockets.transform.position);
            
            StartCoroutine(LockObjectInSocket(args.interactableObject.transform, _alarmMechanismSockets));
        }
        
        public void OnFacePlaced(SelectEnterEventArgs args)
        {
            _advancement++;
            _isCompleted = true;
            
            PlaySoundAtPosition(_gearInsertSFX, _alarmFaceSockets.transform.position);
            PlaySoundAtPosition(_alarmRepairedSFX, _alarmFaceSockets.transform.position);
            EventBus.OnAlarmRepaired?.Invoke();
            
            DisplayCode();
            StartCoroutine(LockObjectInSocket(args.interactableObject.transform, _alarmFaceSockets));
        }

        private void DisplayCode()
        {
            Vector3 targetLocalEulerHours = _hoursHandTransform.localEulerAngles + _hoursPivotAxis * _hoursAngle;
            _hoursHandTransform.DOLocalRotate(targetLocalEulerHours, _duration).SetEase(_animationCurve);
            
            Vector3 targetLocalEulerMinutes = _minutesHandTransform.localEulerAngles + _minutesPivotAxis * _minutesAngle;
            _minutesHandTransform.DOLocalRotate(targetLocalEulerMinutes, _duration).SetEase(_animationCurve);
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

        private void PlaySoundAtPosition(EventReference sfx, Vector3 position)
        {
            if (AudioManager.Instance && !sfx.IsNull)
            {
                AudioManager.Instance.PlayAtPosition(sfx, position);
            }
        }
    }
}