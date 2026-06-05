using System.Collections.Generic;
using Core.Audio;
using DG.Tweening;
using FMODUnity;
using Handlers;
using MonoBehiavors;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Managers
{
    public class CodePadLockHandler : PadLock
    {
        [Header("===== SETTINGS =====")]
        [SerializeField] private List<string> _rightCodes = new();
        [SerializeField] private string _currentCode = "";
        [SerializeField] private List<int> _currentNumbers = new();
        
        [Header("===== REFERENCES =====")]
        [SerializeField] private List<DynamicGear> _gears = new();
        
        [Header("===== VISUAL =====")]
        [Header("-- SMALL PADLOCK --")]
        [SerializeField] private GameObject _lockSmall;
        [SerializeField] private Rigidbody _smallRb;
        [Header("-- BIG PADLOCK --")]
        [SerializeField] private GameObject CodePadLock;
        [SerializeField] private GameObject _lock;
        [SerializeField] private List<XRSimpleInteractable> _gearInteractables = new();
        
        [Header("===== ANIMATION =====")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _animationCurve;

        [Header("===== FMOD AUDIO =====")] 
        [SerializeField] private EventReference _gearTickSFX;
        [SerializeField] private EventReference _unlockSFX;
    
        private bool _bigPadLockSpawned;

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

            _currentNumbers.Clear();
            foreach (var gear in _gears)
            {
                _currentNumbers.Add(0);
            }
            SetCode();
            
            foreach (var gearInteractable in _gearInteractables)
            {
                if (gearInteractable != null) gearInteractable.enabled = false;
            }
        }

        private void Update()
        {
            if (_bigPadLockSpawned)
            {
                if (Vector3.Distance(Camera.main.transform.position, CodePadLock.transform.position) > 1.5f)
                {
                    DespawnBigPadLock();
                }
            }
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            for (int i = 0; i < _gears.Count; i++)
            {
                if (_gears[i] != null) _gears[i].CodeChanged += SetNumber;
            }
        }
        
        private void OnDisable()
        {
            for (int i = 0; i < _gears.Count; i++)
            {
                if (_gears[i] != null) _gears[i].CodeChanged -= SetNumber;
            }
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
                        if (gearInteractable != null) gearInteractable.enabled = true;
                    }
                });
        }

        public void DespawnBigPadLock()
        {
            _bigPadLockSpawned = false;
            
            if (_padLockSequence != null && _padLockSequence.IsActive())
            {
                _padLockSequence.Kill();
            }
            
            foreach (var gearInteractable in _gearInteractables)
            {
                if (gearInteractable != null) gearInteractable.enabled = false;
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
                        if (_smallRb != null) _smallRb.isKinematic = false;
                        UnityEvent?.Invoke();
                    }
                });
        }

        #endregion

        #region ===== PRIVATE METHODS =====

        // CORRECTION : Plus de .IndexOf(), on reçoit directement l'identifiant propre de la roue
        private void SetNumber(int gearIndex, int value)
        {
            if (gearIndex < 0 || gearIndex >= _currentNumbers.Count) return;

            _currentNumbers[gearIndex] = value;

            if (AudioManager.Instance && !_gearTickSFX.IsNull && gearIndex < _gears.Count)
            {
                if (_gears[gearIndex] != null)
                {
                    AudioManager.Instance.Play(_gearTickSFX, loop: false, follow: _gears[gearIndex].gameObject);
                }
            }
            SetCode();
        }
        
        private void SetCode()
        {
            _currentCode = "";
            foreach (var number in _currentNumbers)
            {
                _currentCode += number;
            }
            VerifyIfCodeIsRight();
        }
    
        private void VerifyIfCodeIsRight()
        {
            foreach (var code in _rightCodes)
            {
                if (code == _currentCode)
                {
                    OpenLockPad();
                    return;
                }
            }
        }

        private void OpenLockPad()
        {
            IsLock = false;
            
            if (AudioManager.Instance && !_unlockSFX.IsNull)
            {
                AudioManager.Instance.Play(_unlockSFX, loop: false, follow: gameObject);
            }
            
            AnimationUnlock();
        }

        private void AnimationUnlock()
        {
            if (_lock != null)
            {
                Vector3 rotBig = _lock.transform.rotation.eulerAngles;
                Vector3 newRotBig = new Vector3(rotBig.x, rotBig.y, rotBig.z - 30f);
                _lock.transform.DORotate(newRotBig, 1f).OnComplete(DespawnBigPadLock);
            }

            if (_lockSmall != null)
            {
                Vector3 rotSmall = _lockSmall.transform.rotation.eulerAngles;
                Vector3 newRotSmall = new Vector3(rotSmall.x, rotSmall.y, rotSmall.z - 30f);
                _lockSmall.transform.DORotate(newRotSmall, 1f);
            }
        }

        #endregion
    }
}