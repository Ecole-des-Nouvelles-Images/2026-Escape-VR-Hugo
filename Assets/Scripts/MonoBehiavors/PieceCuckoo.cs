using UnityEngine;

namespace MonoBehiavors
{
    public class PieceCuckoo : MonoBehaviour
    {
        private GameObject _slotCuckoo;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("SlotCuckoo"))
            {
                _slotCuckoo =  other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("SlotCuckoo"))
            {
                _slotCuckoo = null;
            }
        }

        public void AttachToSlot()
        {
            if (!_slotCuckoo) return;
            //_slotCuckoo.GetComponent<SlotCuckoo>().AttachPiece(gameObject);
        }

        public void UnAttachToSlot()
        {
            if (!_slotCuckoo) return;
            //_slotCuckoo.GetComponent<SlotCuckoo>().UnAttachPiece(gameObject);
        }
    }
}
