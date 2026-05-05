using Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace _Branches.Hugo.Scripts.PuzzleFlower
{
    public class AlarmClockHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private XRSocketInteractor _alarmMechanismSockets;
        [SerializeField] private XRSocketInteractor _alarmFaceSockets;
        
        [Header("===== DEBUG =====")]
        [SerializeField] private bool _isCompleted;

        private void Awake()
        {
            _alarmMechanismSockets.enabled = true;
            _alarmFaceSockets.enabled = false;
        }

        public void OnMechanismPlaced(SelectEnterEventArgs args)
        {
            _alarmFaceSockets.enabled = true;
            
            Transform go = args.interactableObject.transform;
            
            XRGrabInteractable goGrab = go.GetComponent<XRGrabInteractable>();
            goGrab.enabled = false;
            
            _alarmMechanismSockets.enabled = false;
            SetParent(_alarmMechanismSockets.transform, go);
        }
        
        public void OnFacePlaced(SelectEnterEventArgs args)
        {
            _isCompleted = true;
            EventBus.OnAlarmRepaired?.Invoke();
            
            Transform go = args.interactableObject.transform;
            
            XRGrabInteractable goGrab = go.GetComponent<XRGrabInteractable>();
            goGrab.enabled = false;
            
            _alarmFaceSockets.enabled = false;
            SetParent(_alarmFaceSockets.transform, go);
        }

        private void SetParent(Transform parent, Transform child)
        {
            child.transform.SetParent(parent);
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
        }
    }
}