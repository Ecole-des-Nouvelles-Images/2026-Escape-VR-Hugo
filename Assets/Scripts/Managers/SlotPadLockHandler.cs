using Core.Audio;
using DG.Tweening;
using FMODUnity;
using MonoBehiavors;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Managers
{
   public class SlotPadLockHandler : PadLock
   {
      [SerializeField] private GameObject _keyObject;

      [SerializeField] private Vector3 OffSetPositionOnKey;
      [SerializeField] private Vector3 OffSetRotationOnKey;

      [SerializeField] private GameObject _lock;
      [SerializeField] private GameObject _socketTransform;
      [SerializeField] private BoxCollider _lockCollider;

      [Header("===== FMOD AUDIO =====")] 
      [SerializeField] private EventReference _keyInsertSFX;
      [SerializeField] private EventReference _keyTurnSFX;
      [SerializeField] private EventReference _lockOpenSFX;
      private void OnTriggerEnter(Collider other)
      {
         // if (other.gameObject == _keyObject)
         // {
         //    if(IsLock) UnlockPadLock();
         // }
      }

      public void UnlockPadLock()
      {
         base.UnlockPadLock();
         Debug.Log("UnlockPadLock");
         _keyObject.GetComponent<BoxCollider>().enabled = false;
         _keyObject.transform.parent = transform;
         AnimatedKey();
      }

      [ContextMenu("AnimKey")]
      private void AnimatedKey()
      {
         Debug.Log("Animated Key");
         _keyObject.GetComponent<BoxCollider>().enabled = false;
         
         if (AudioManager.Instance && !_keyInsertSFX.IsNull)
         {
            AudioManager.Instance.Play(_keyInsertSFX, loop: false, follow: _socketTransform);
         }

         _socketTransform.transform.DOLocalMoveZ(_socketTransform.transform.localPosition.z + 0.03f, 1f).OnComplete(() =>
         {
            Debug.Log("Rotation");
            
            if (AudioManager.Instance && !_keyTurnSFX.IsNull)
            {
               AudioManager.Instance.Play(_keyTurnSFX, loop: false, follow: _socketTransform);
            }
    
            _socketTransform.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.5f, RotateMode.LocalAxisAdd)
               .OnComplete(() =>
               {
                  PadLockAnimation();
               });
         });
      }
      
      [ContextMenu("Animated Padlock")]
      private void PadLockAnimation()
      {
         if (AudioManager.Instance && !_lockOpenSFX.IsNull)
         {
            AudioManager.Instance.Play(_lockOpenSFX, loop: false, follow: _lock);
         }
         
         _lock.transform.DOMoveY(_lock.transform.position.y + 0.01f, 0.5f).OnComplete(() =>
         {
            Rigidbody rb = GetComponent<Rigidbody>(); 
            rb.isKinematic = false;
            _lockCollider.enabled = true;
            UnityEvent?.Invoke();
         });
      }
   }
}
