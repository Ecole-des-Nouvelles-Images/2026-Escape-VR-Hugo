using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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
    

    public void AttachPiece(GameObject piece)
    {
        if(_slotOccuped) return;
        if (piece == _rightPiece)
        {
            ValidPiece(); 
            return;
        }
        
        Debug.Log("AttachPiece");
        piece.GetComponent<Rigidbody>().isKinematic = true;
        
        piece.transform.position = transform.position;
        piece.transform.localRotation = new Quaternion(0,0,0,0);
        
        _slotOccuped = true;
        IsValid = false;
    }

    public void UnAttachPiece(GameObject piece)
    {
        Debug.Log("UnAttachPiece");
        _slotOccuped = false;
        IsValid = false;
        _cuckoo.InvalidSlot(piece);
        piece.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void ValidPiece()
    {
        Debug.Log("AttachRightPiece");
        
        _rightPiece.transform.position = transform.position;
        _rightPiece.transform.localRotation = new Quaternion(0,0,0,0);
        
        _rightPiece.GetComponent<Rigidbody>().isKinematic = true;
        
        _cuckoo.ValidSlot(_rightPiece);
        
        _slotOccuped = true;
        IsValid = true;
    }

    
}
