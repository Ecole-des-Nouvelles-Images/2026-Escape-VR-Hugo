using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CuckooHandler : MonoBehaviour
{
    [SerializeField] private int _SlotValid;
    [SerializeField] private GameObject _bird;
    
    [SerializeField] private List<GameObject> _piecesValid = new List<GameObject>();
    
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
    
    private void InsertAllPieces()
    {
        Debug.Log("Insert all pieces");
        StartCoroutine("Timer");
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        
        List<GameObject> piecesCopy = new List<GameObject>(_piecesValid);
        
        foreach (GameObject piece in piecesCopy)
        {
            piece.GetComponent<XRGrabInteractable>().enabled = false;
            piece.GetComponent<Rigidbody>().isKinematic = true;
            piece.transform.DOMoveY(piece.transform.position.y + 0.05f, 0.5f);
        }
        EventBus.OnCuckooClockRepaired?.Invoke();
        
        yield return new WaitForSeconds(1.5f);
        
        _bird.SetActive(true);
        EventBus.OnCandleKeyUnlocked?.Invoke();
    }
}
