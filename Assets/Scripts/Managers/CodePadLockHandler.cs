using System.Collections.Generic;
using DG.Tweening;
using Handlers;
using MonoBehiavors;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Managers
{
    public class CodePadLockHandler : PadLock
    {
        [Header("===== SETTINGS =====")]
        [SerializeField] private string RightCode;
        [SerializeField] private string CurrentCode;
        [SerializeField] private int NumberOne;
        [SerializeField] private int NumberTwo;
        
        [Header("===== VISUAL =====")]
        [Header("-- SMALL PADLOCK --")]
        [SerializeField] private GameObject _lockSmall;
        [SerializeField] private Rigidbody _smallRb;
        [Header("-- BIG PADLOCK --")]
        [SerializeField] private GameObject CodePadLock;
        [SerializeField] private DynamicGear _gearOne;
        [SerializeField] private DynamicGear _gearTwo;
        [SerializeField] private GameObject _lock;
        [SerializeField] private List<XRSimpleInteractable> _gearInteractables = new();
        
        [Header("===== ANIMATION =====")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _animationCurve;
    
        private bool _bigPadLockSpawned;

        // DISPLAY
        private Sequence _padLockSequence;
        private Transform _bigVisualTransform;
        private Vector3 _bigVisualPos;
        private Quaternion _bigVisualRot;
        private Vector3 _bigVisualScale;

        private void Awake()
        {
            _bigVisualTransform = CodePadLock.transform;
            _bigVisualPos = _bigVisualTransform.position;
            _bigVisualRot = _bigVisualTransform.rotation;
            _bigVisualScale = _bigVisualTransform.localScale;
            
            foreach (var gearInteractable in _gearInteractables)
            {
                gearInteractable.enabled = false;
            }
        }

        private void Update()
        {
            if (_bigPadLockSpawned)
            {
                if (Vector3.Distance(Camera.main.gameObject.transform.position, CodePadLock.transform.position) > 2.5f)
                {
                    DespawnBigPadLock();
                }
            }
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            _gearOne.CodeChanged += SetNumberOne;
            _gearTwo.CodeChanged += SetNumberTwo;
        }
        
        private void OnDisable()
        {
            _gearOne.CodeChanged -= SetNumberOne;
            _gearTwo.CodeChanged -= SetNumberTwo;
        }

        #endregion

        #region ===== PUBLIC METHODS =====

        public void Interact()
        {
            if (!IsLock) return;
            PadLockManager.Instance.SetCurrentPadLock(this, _bigPadLockSpawned);
        }
        
        public void SpawnBigPadLock()
        {
            _bigPadLockSpawned = true;
            
            // DISPLAY
            if (_padLockSequence != null && _padLockSequence.IsActive())
            {
                _padLockSequence.Kill();
            }
            
            _bigVisualTransform.SetPositionAndRotation(transform.position, transform.rotation);
            CodePadLock.transform.localScale = Vector3.one;
            
            CodePadLock.SetActive(true);

            _padLockSequence = DOTween.Sequence()
                .Join(_bigVisualTransform.DOMove(_bigVisualPos, _duration))
                .Join(_bigVisualTransform.DORotateQuaternion(_bigVisualRot, _duration))
                .Join(_bigVisualTransform.DOScale(_bigVisualScale, _duration))
                .SetEase(_animationCurve)
                .OnComplete(() =>
                {
                    foreach (var gearInteractable in _gearInteractables)
                    {
                        gearInteractable.enabled = true;
                    }
                });
        }

        public void DespawnBigPadLock()
        {
            _bigPadLockSpawned = false;
            
            // DISPLAY
            if (_padLockSequence != null && _padLockSequence.IsActive())
            {
                _padLockSequence.Kill();
            }
            
            foreach (var gearInteractable in _gearInteractables)
            {
                gearInteractable.enabled = false;
            }
            
            _padLockSequence = DOTween.Sequence()
                .Join(_bigVisualTransform.DOMove(transform.position, _duration))
                .Join(_bigVisualTransform.DORotateQuaternion(transform.rotation, _duration))
                .Join(_bigVisualTransform.DOScale(transform.localScale, _duration))
                .SetEase(_animationCurve)
                .OnComplete(() =>
                {
                    CodePadLock.SetActive(false);
                    
                    _bigVisualTransform.SetPositionAndRotation(_bigVisualPos, _bigVisualRot);
                    CodePadLock.transform.localScale = _bigVisualScale;

                    if (!IsLock)
                    {
                        _smallRb.isKinematic = false;
                    }
                });
        }

        #endregion

        #region ===== PRIVATE METHODS =====

        private void SetNumberOne(int value)
        {
            NumberOne = value;
            SetCode();
        }
        private void SetNumberTwo(int value)
        {
            NumberTwo = value; 
            SetCode();
        }
        
        private void SetCode()
        {
            CurrentCode = new string(NumberOne + "" + NumberTwo);
            VerifyIfCodeIsRight();
        }
    
        private void VerifyIfCodeIsRight()
        {
            if (RightCode == CurrentCode)
            {
                OpenLockPad();
            }
        }

        private void OpenLockPad()
        {
            IsLock = false;
            UnityEvent?.Invoke();
            
            AnimationUnlock();
        }

        private void AnimationUnlock()
        {
            // BIG
            Vector3 rotBig =  _lock.transform.rotation.eulerAngles;
            Vector3 newRotBig = new Vector3(rotBig.x ,rotBig.y, rotBig.z + -30);
            _lock.transform.DORotate(newRotBig ,1)
                .OnComplete(DespawnBigPadLock);
            
            // SMALL
            Vector3 rotSmall =  _lockSmall.transform.rotation.eulerAngles;
            Vector3 newRotSmall = new Vector3(rotSmall.x ,rotSmall.y, rotSmall.z + -30);
            _lockSmall.transform.DORotate(newRotSmall, 1);
        }

        #endregion
    }
}
