using System.Collections.Generic;
using DG.Tweening;
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
        [Header("-- BIG PADLOCK --")]
        [SerializeField] private GameObject CodePadLock;
        [SerializeField] private GameObject _gearOne;
        [SerializeField] private GameObject _gearTwo;
        [SerializeField] private GameObject _lock;
        [SerializeField] private List<XRGrabInteractable> _gearInteractables = new();
        
        [Header("===== ANIMATION =====")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _animationCurve;
    
        private bool _bigPadLockSpawned;
        private bool _canRotateGear = true;

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

        [ContextMenu("Interact")]
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
                });
        }

        #region RegionModifyCode

        public void AddNumberOne()
        {
            if (!_canRotateGear) return;
            NumberOne++;
            if (NumberOne < 0) NumberOne = 9;
            if (NumberOne > 9) NumberOne = 0;
            SetCode();
            RotateGear(_gearOne, 36);
        }
        public void RemoveNumberOne()
        {
            if (!_canRotateGear) return;
            NumberOne--; 
            if (NumberOne < 0) NumberOne = 9;
            if (NumberOne > 9) NumberOne = 0;
            SetCode();
            RotateGear(_gearOne, -36);
        }
    
        public void AddNumberTwo()
        {
            if (!_canRotateGear) return;
            NumberTwo++;
            if (NumberTwo < 0) NumberTwo = 9;
            if (NumberTwo > 9) NumberTwo = 0;
            SetCode();
            RotateGear(_gearTwo, 36);
        }
        public void RemoveNumberTwo()
        {
            if (!_canRotateGear) return;
            NumberTwo--;
            if (NumberTwo < 0) NumberTwo = 9;
            if (NumberTwo > 9) NumberTwo = 0;
            SetCode();
            RotateGear(_gearTwo, -36);
        }
    
        #endregion

        private void SetCode()
        {
            CurrentCode = new string(NumberOne + "" + NumberTwo);
            VerifyIfCodeIsRight();
        }
    
        private void VerifyIfCodeIsRight()
        {
            if (RightCode == CurrentCode)
            {
                Debug.Log("Code is good");
                OpenLockPad();
            }
            else
            {
                Debug.Log("Code is bad");
            }
        }

        private void OpenLockPad()
        {
            IsLock = false;
            UnityEvent?.Invoke();
            AnimationUnlock();
        }

        private void RotateGear(GameObject gearTarget, float rotateValue)
        {
            _canRotateGear = false;
            rotateValue = -rotateValue;
            gearTarget.transform.DOLocalRotate(new Vector3(rotateValue, 0f, 0f), 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(() =>
            {
                _canRotateGear = true;
            });
        }

        [ContextMenu("AnimationUnlock")]
        private void AnimationUnlock()
        {
            Vector3 rotation =  _lock.transform.rotation.eulerAngles;
            Vector3 newRotation = new Vector3(rotation.x ,rotation.y, rotation.z + -30);
            _lock.transform.DORotate(newRotation ,1).OnComplete(() =>
            {
                CodePadLock.transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
                {
                    CodePadLock.SetActive(false);
                    AnimateReelPadLock();
                });
            });
        }

        private void AnimateReelPadLock()
        {
            Vector3 rotation =  _lockSmall.transform.rotation.eulerAngles;
            Vector3 newRotation = new Vector3(rotation.x ,rotation.y, rotation.z + -30);
            _lockSmall.transform.DORotate(newRotation, 1).OnComplete(() =>
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            });
        }
    }
}
