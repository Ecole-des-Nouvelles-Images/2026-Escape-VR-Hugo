using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Door : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable _grabInteractable;
    
    public void Open()
    {
        _grabInteractable.enabled = true;
    }

    public void Close()
    {
        _grabInteractable.enabled = false;
    }
}
