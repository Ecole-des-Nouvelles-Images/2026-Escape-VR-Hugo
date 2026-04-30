using System;
using UnityEngine;

public class SlotPadLockHandler : PadLock
{
   [SerializeField] private GameObject _keyObject;

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
      transform.localPosition = Vector3.zero;
   }
}
