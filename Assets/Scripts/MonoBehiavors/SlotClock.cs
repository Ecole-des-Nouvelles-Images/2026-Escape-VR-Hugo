using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MonoBehiavors
{
    public class SlotClock : MonoBehaviour
    {
        [SerializeField] private GameObject _rightKey;
        [SerializeField] private UnityEvent _onRightKeyInsert;
    
        private XRSocketInteractor _XRSocketInteractor;
        private bool _slotOcuped;
    
        private void Awake()
        {
            _XRSocketInteractor = GetComponent<XRSocketInteractor>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _rightKey)
            {
                _XRSocketInteractor.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _rightKey)
            {
                _XRSocketInteractor.enabled = false;
            }
        }

        public void InsertKey(SelectEnterEventArgs arg)
        {
            GameObject obj = arg.interactableObject.transform.gameObject;

            if (_slotOcuped) return;
        
            if (obj == _rightKey)
            {
                Debug.Log("InsertKey");
                obj.GetComponent<Rigidbody>().isKinematic = true;
                StartCoroutine("Animation");
                _slotOcuped = true;
            }
        }

        public IEnumerator Animation()
        {
            yield return new WaitForSeconds(0.3f);
            transform.DOMoveX(transform.position.x - 0.07f, 1).OnComplete(() =>
            {
                Vector3 rotation = transform.rotation.eulerAngles;
                rotation.x += 90;
                transform.DORotate(rotation, 0.1f).OnComplete((() =>
                {
                    _onRightKeyInsert?.Invoke();
                }));
            });
        }
    }
}
