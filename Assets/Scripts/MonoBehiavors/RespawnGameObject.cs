using System.Collections;
using UnityEngine;

namespace MonoBehiavors
{
    public class RespawnGameObject : MonoBehaviour
    {
        private Vector3 _startPos;
        private Quaternion _startRot;
        private Rigidbody _rigidbody;
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _startRot = transform.rotation;
            _startPos = transform.position;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("LimitMap"))
            {
                StartCoroutine(RespawnObject());
            }
        }

        private IEnumerator RespawnObject()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = _startPos;
            transform.rotation = _startRot;
        
            yield return new WaitForSeconds(0.1f);
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}