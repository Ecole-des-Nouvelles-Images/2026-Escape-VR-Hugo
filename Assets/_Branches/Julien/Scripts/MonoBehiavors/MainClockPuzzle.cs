using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class MainClockPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject _rightKey;
    [SerializeField] private UnityEvent _onKeyInsert;
    
    public void InsertKey(SelectEnterEventArgs arg)
    {
        GameObject obj = arg.interactableObject.transform.gameObject;
        
        if (obj == _rightKey)
        {
            
        }
    }
}
