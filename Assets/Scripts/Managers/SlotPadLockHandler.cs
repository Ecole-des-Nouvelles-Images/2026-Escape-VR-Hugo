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
   
      private void OnTriggerEnter(Collider other)
      {
         // if (other.gameObject == _keyObject)
         // {
         //    if(IsLock) UnlockPadLock();
         // }
      }

      public void UnlockPadLock(SelectEnterEventArgs args)
      {
         base.UnlockPadLock();
         _keyObject.GetComponent<BoxCollider>().enabled = false;
         transform.parent = _keyObject.transform;
         transform.localPosition = Vector3.zero +  OffSetPositionOnKey;
         transform.localRotation = new Quaternion(OffSetRotationOnKey.x, OffSetRotationOnKey.y, OffSetRotationOnKey.z, Quaternion.identity.w);
      }
   }
}
