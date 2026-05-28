using DG.Tweening;
using UnityEngine;

public class UnlockKnob : MonoBehaviour
{
    public Transform KnobTransform;
    
    [ContextMenu("Unlock")]
    public void Unlock()
    {
        KnobTransform.transform.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f, RotateMode.LocalAxisAdd);
    }
}
