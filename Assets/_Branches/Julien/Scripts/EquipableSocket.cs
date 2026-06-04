using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipableSocket : MonoBehaviour
{
    public void OnEquipementEnter(SelectEnterEventArgs arg)
    {
        GameObject o = arg.interactableObject.transform.gameObject;
        o.GetComponent<EquipableObject>().DisableCollider();
    }

    public void OnEquipementExit(SelectExitEventArgs arg)
    {
        GameObject o = arg.interactableObject.transform.gameObject;
        o.GetComponent<EquipableObject>().EnableCollider();
    }
}
