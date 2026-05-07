using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace _Branches.Hugo.Scripts
{
    public abstract class SocketHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] protected XRSocketInteractor _conectedSocket;
        [SerializeField] protected float _disableDelay = 0.1f;

        private void Start()
        {
            _conectedSocket.enabled = false;
        }

        public abstract void OnSelectedEnter();

        public void OnSelectedExit()
        {
            StartCoroutine(DisableSocketRoutine());
        }

        private IEnumerator DisableSocketRoutine()
        {
            yield return new WaitForSeconds(_disableDelay);

            if (_conectedSocket && _conectedSocket.interactablesSelected.Count == 0)
            {
                _conectedSocket.enabled = false;
            }
        }
    }
}