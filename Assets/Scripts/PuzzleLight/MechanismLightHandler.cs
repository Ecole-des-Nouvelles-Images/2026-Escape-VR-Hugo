using Core.Audio;
using DG.Tweening;
using FMODUnity;
using UnityEngine;

namespace PuzzleLight
{
    public class MechanismLightHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private MechanismLightReceiver _mechanismLightReceiver;
        [SerializeField] private PuzzleLightHatchHandler _puzzleLightHatchHandler;
        [SerializeField] private Transform _doorTransform;
        [SerializeField] private Transform _drawerTransform;
    
        [Header("===== SETTINGS DOOR =====")]
        [SerializeField] private Vector3 _pivotAxis = Vector3.up;
        [SerializeField] private float _openAngle = 90f;
        
        [Header("===== SETTINGS DRAWER =====")] 
        [SerializeField] private Vector3 _drawerOffset = new(0, 0, 0.5f);
        
        [Header("===== ANIMATION =====")]
        [SerializeField] private float _openDuration = 2f;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("===== SFX =====")]
        [SerializeField] private EventReference _doorOpenSFX;
        [SerializeField] private EventReference _doorCloseSFX;
        [SerializeField] private EventReference _drawerOpenSFX;
        [SerializeField] private EventReference _drawerCloseSFX;

        private Vector3 _closedDoorLocalEuler;
        private Vector3 _closedDrawerPos; 
        
        private Sequence _mechanismSequence;

        private void Start()
        {
            _closedDoorLocalEuler = _doorTransform.localEulerAngles;
            _closedDrawerPos = _drawerTransform.localPosition;
        }
        
        [ContextMenu("RotateDoorTest")]
        public void RotateDoorTest()
        {
            Vector3 targetLocalEuler = _closedDoorLocalEuler + _pivotAxis * _openAngle;
            _doorTransform.DOLocalRotate(targetLocalEuler, _openDuration).SetEase(_animationCurve);
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            if (_mechanismLightReceiver != null)
                _mechanismLightReceiver.OnLit += RotateDoor;
        }

        private void OnDisable()
        {
            if (_mechanismLightReceiver != null)
                _mechanismLightReceiver.OnLit -= RotateDoor;
            
            _mechanismSequence?.Kill();
        }
        
        private void RotateDoor(bool isLit)
        {
            DOTween.Kill(_doorTransform);
            DOTween.Kill(_drawerTransform);
            _mechanismSequence?.Kill();
            _mechanismSequence = DOTween.Sequence();

            if (isLit)
            {
                Vector3 targetLocalEuler = _closedDoorLocalEuler + _pivotAxis * _openAngle;

                if (!_puzzleLightHatchHandler.IsResolved)
                {
                    _mechanismSequence.Append(_doorTransform.DOLocalRotate(targetLocalEuler, _openDuration).SetEase(_animationCurve));
                    
                    _mechanismSequence.OnStart(() => PlaySound(_doorOpenSFX));
                }
                else
                {
                    _mechanismSequence.Append(_doorTransform.DOLocalRotate(targetLocalEuler, _openDuration).SetEase(_animationCurve));
                    _mechanismSequence.Append(_drawerTransform.DOLocalMove(_closedDrawerPos + _drawerOffset, _openDuration).SetEase(_animationCurve));

                    _mechanismSequence.OnStart(() => PlaySound(_doorOpenSFX));
                    _mechanismSequence.InsertCallback(_openDuration, () => PlaySound(_drawerOpenSFX));
                    

                }
            }
            else
            {
                if (!_puzzleLightHatchHandler.IsResolved)
                {
                    _mechanismSequence.Append(_doorTransform.DOLocalRotate(_closedDoorLocalEuler, _openDuration).SetEase(_animationCurve));
                    
                    _mechanismSequence.OnStart(() => PlaySound(_doorCloseSFX));
                }
                else
                {
                    _mechanismSequence.Append(_drawerTransform.DOLocalMove(_closedDrawerPos, _openDuration).SetEase(_animationCurve));
                    _mechanismSequence.Append(_doorTransform.DOLocalRotate(_closedDoorLocalEuler, _openDuration).SetEase(_animationCurve));
                    
                    _mechanismSequence.OnStart(() => PlaySound(_drawerCloseSFX));
                    _mechanismSequence.InsertCallback(_openDuration, () => PlaySound(_doorCloseSFX));
                }
            }
            
            _mechanismSequence.Play();
            
            // SFX
            AudioManager.Instance.Play(_doorOpenSFX);
        }

        #endregion

        private void PlaySound(EventReference sfx)
        {
            if (AudioManager.Instance && !sfx.IsNull)
            {
                AudioManager.Instance.PlayAtPosition(sfx, transform.position);
            }
        }
    }
}