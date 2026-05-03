using System;
using UnityEngine;

public class SlotPadLockHandler : PadLock
{
   [SerializeField] private GameObject _keyObject;

   [SerializeField] private Vector3 OffSetPositionOnKey;
   [SerializeField] private Vector3 OffSetRotationOnKey;
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject == _keyObject)
      {
         if(IsLock) UnlockPadLock();
      }
   }

   protected override void UnlockPadLock()
   {
      base.UnlockPadLock();
      transform.parent = _keyObject.transform;
      transform.localPosition = Vector3.zero +  OffSetPositionOnKey;
      transform.localRotation = new Quaternion(OffSetRotationOnKey.x, OffSetRotationOnKey.y, OffSetRotationOnKey.z, Quaternion.identity.w);
   }
}
