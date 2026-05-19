using Handlers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MonoBehiavors
{
    public class SlotCuckoo : MonoBehaviour
    {
        [SerializeField] private GameObject _rightPiece;
        private bool _slotOccuped;
        public bool IsValid;
    
        private CuckooHandler _cuckoo;

        private void Awake()
        {
            _cuckoo = GetComponentInParent<CuckooHandler>();
        }
    
        public void AttachPiece(SelectEnterEventArgs arg)
        {
            GameObject piece = arg.interactableObject.transform.gameObject;
        
            if(_slotOccuped) return;
            Debug.Log(piece.name);
            if (piece == _rightPiece)
            {
                ValidPiece(); 
                return;
            }
        
            piece.GetComponent<Rigidbody>().isKinematic = true;
        
            _slotOccuped = true;
            IsValid = false;
        }

        public void UnAttachPiece(SelectExitEventArgs arg)
        {
            if (arg == null) return;
            GameObject piece = arg.interactableObject.transform.gameObject;
        
            _slotOccuped = false;
            IsValid = false;
            _cuckoo.InvalidSlot(piece);
            piece.GetComponent<Rigidbody>().isKinematic = false;
        }

        public void ValidPiece()
        {
            _rightPiece.transform.position = transform.position;
            _rightPiece.transform.localRotation = new Quaternion(0,0,0,0);
        
            _rightPiece.GetComponent<Rigidbody>().isKinematic = true;
        
            _cuckoo.ValidSlot(_rightPiece);
        
            _slotOccuped = true;
            IsValid = true;
        }

    
    }
}
