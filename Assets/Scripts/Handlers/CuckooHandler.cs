using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Handlers
{
    public class CuckooHandler : MonoBehaviour
    {
        [SerializeField] private int _SlotValid;
    
        [SerializeField] private List<GameObject> _piecesValid = new List<GameObject>();
        [SerializeField] private List<GameObject> _slotCorchet = new List<GameObject>();
        
        [Header("SFX")] 
        [SerializeField] private EventReference _cuckooSound;
    
        [SerializeField] private Animator _animation;

        private void Start()
        {
            
        }

        public void ValidSlot(GameObject piece)
        {
            Debug.Log("validSlot");
            _SlotValid++;
            _piecesValid.Add(piece);
            Debug.Log(_SlotValid);
            if (_SlotValid == 4)
            {
                InsertAllPieces();
            }
        }

        public void InvalidSlot(GameObject piece)
        {
            if (_piecesValid.Contains(piece))
            {
                _piecesValid.Remove(piece);
                _SlotValid--;
            }
        }
    
        [ContextMenu("MoveCrochet")]
        private void InsertAllPieces()
        {
            Debug.Log("Insert all pieces");
            StartCoroutine("Timer");
        }

        
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(1f);
        
            List<GameObject> crochets = new List<GameObject>(_slotCorchet);
            foreach (GameObject crochet in crochets)
            {
                crochet.transform.DOMoveY(crochet.transform.position.y + 0.02f, 0.5f);
            }
            
            List<GameObject> piecesCopy = new List<GameObject>(_piecesValid);
            foreach (GameObject piece in piecesCopy)
            {
                piece.transform.DOMoveY(piece.transform.position.y + 0.02f, 0.5f);
                piece.GetComponent<XRGrabInteractable>().enabled = false;
                piece.GetComponent<Rigidbody>().isKinematic = true;
            }
            EventBus.OnCuckooClockRepaired?.Invoke();
            _animation.SetTrigger("PlayAnimation");
            
            yield return new WaitForSeconds(1.5f);
            
        
            EventBus.OnCandleKeyUnlocked?.Invoke();
        }
    }
}
