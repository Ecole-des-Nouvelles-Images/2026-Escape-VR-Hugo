using System.Collections;
using UnityEngine;

namespace MonoBehiavors
{
    public class ParentHandler : MonoBehaviour
    {
        public void RemoveParent()
        {
            StartCoroutine(RemoveParentRoutine());
        }

        private IEnumerator RemoveParentRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            transform.parent = null;
        }
    }
}