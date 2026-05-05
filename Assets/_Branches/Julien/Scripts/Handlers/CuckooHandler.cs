using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CuckooHandler : MonoBehaviour
{
    [SerializeField] private int _SlotValid;
    [SerializeField] private GameObject _bird;
    
    private List<GameObject> _piecesValid = new List<GameObject>();
    
    public void ValidSlot(GameObject piece)
    {
        Debug.Log("validSlot");
        _SlotValid++;
        _piecesValid.Add(piece);
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
        
        foreach (GameObject piece in _piecesValid)
        {
            piece.GetComponent<XRGrabInteractable>().enabled = false;
            piece.GetComponent<Rigidbody>().isKinematic = true;
            
            piece.transform.DOMoveY(piece.transform.position.y + 0.2f, 0.9f);
        }

        StartCoroutine("Timer");
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        CoucouAnimation();
    }

    [ContextMenu("COUCOU")]
    private void CoucouAnimation()
    {
        _bird.SetActive(true);
    }
}
