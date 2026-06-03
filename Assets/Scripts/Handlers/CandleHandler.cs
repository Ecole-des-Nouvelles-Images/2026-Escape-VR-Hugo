using System.Collections.Generic;
using System.Linq;
using Core;
using Managers;
using MonoBehiavors;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Handlers
{
    public class CandleHandler : TemporalGameObject
    {
        [Header("==== Candle ====")] 
        [SerializeField] private GameObject _candleVisual;

        [Header("Fire")] 
        public bool IsFire;
        [SerializeField] private bool _fireInStart;
        [SerializeField] private GameObject _flameGameObject;
        [SerializeField] private GameObject _flameVisual;
        [SerializeField] private Transform _firePoint;
    
        [Header("ObjectInside")] 
        [SerializeField] private float _valueDrop;
        [SerializeField] private GameObject _objectToDrop;
        [SerializeField] private bool _dropedHisObject;
        [SerializeField] private bool _dropedObjectWasInteracted;
    
        private Vector3 _objectBasePosition;
        private Vector3 _objectBaseRotation;
    
        private void Start()
        {
            if (_objectToDrop != null)
            {
                _objectBasePosition = _objectToDrop.transform.localPosition;
                _objectBaseRotation = _objectToDrop.transform.localEulerAngles;
            }
            if (_fireInStart) Fire();
        }

        [ContextMenu("Fire")]
        public void Fire()
        {
            if (IsFire) return;
            IsFire = true;
            _flameVisual.SetActive(true);
            for (int i = 0; i < _flameVisual.transform.childCount; i++) { _flameVisual.transform.GetChild(i).gameObject.SetActive(true); }
            float currentTime = ClockTimeManager.Instance.NormalizedCurrentTime;
        
            if(!_fireInStart) _temporalRange = new Vector2(currentTime, currentTime + 0.3f);
        }
    
        [ContextMenu("BlowOut")]
        private void BlowOut()
        {
            List<FlameAnimation> flames = _flameVisual.GetComponentsInChildren<FlameAnimation>().ToList();
            foreach (var flame in flames) { flame.BlowOut(); }
            //_flameVisual.GetComponent<FlameAnimation>().BlowOut();
            IsFire = false;
            _temporalRange = Vector2.zero;
        }
        
        protected override void TimeBehavior()
        {
            _candleVisual.transform.localScale = new Vector3(1, 1 - _state, 1);
            if (_flameGameObject) _flameGameObject.transform.position = _firePoint.position;

            if (_state >= _valueDrop && !_dropedHisObject)
            {
                DropObjectInCandle();
            }
            else if (_state < _valueDrop && _dropedHisObject && !_dropedObjectWasInteracted)
            {
                PutObjectInCandle();
            }
            if (!IsFire) return;

            float currentTime = ClockTimeManager.Instance.NormalizedCurrentTime;

            if (currentTime < _temporalRange.x)
            {
                BlowOut();
            }
            else if (currentTime > _temporalRange.y)
            {
                if (_flameGameObject.activeSelf) _flameGameObject.SetActive(false);
            }
            else
            {
                if (!_flameGameObject.activeSelf) _flameGameObject.SetActive(true);
            }
        }
        

        private void DropObjectInCandle()
        {
            if (_objectToDrop == null) return;
        
            _objectToDrop.transform.parent = null;
            
            _objectToDrop.GetComponent<XRGrabInteractable>().enabled = true;
            _objectToDrop.GetComponent<BoxCollider>().isTrigger = false;
            
            Rigidbody rb = _objectToDrop.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            _dropedHisObject = true;
        }

        private void PutObjectInCandle()
        {
            if (_objectToDrop == null) return;

            _objectToDrop.transform.parent = transform;
            _objectToDrop.GetComponent<XRGrabInteractable>().enabled = false;
            _objectToDrop.GetComponent<BoxCollider>().isTrigger = true ;
        
            Rigidbody rb = _objectToDrop.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

            _objectToDrop.transform.localPosition = _objectBasePosition;
            _objectToDrop.transform.localEulerAngles = _objectBaseRotation;

            _dropedHisObject = false;
        }

        public void OnObjectGrabbed()
        {
            _dropedObjectWasInteracted = true;
            if (_objectToDrop.CompareTag("Key")) EventBus.OnFirstKeyUnlocked?.Invoke();
        }
    }
}