using Core.Audio;
using DG.Tweening;
using FMODUnity;
using UnityEngine;

public class UnlockKnob : MonoBehaviour
{
    public Transform KnobTransform;
    
    [Header("===== FMOD AUDIO =====")]
    [SerializeField] private EventReference _unlockSFX;
    
    [ContextMenu("Unlock")]
    public void Unlock()
    {
        KnobTransform.transform.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f, RotateMode.LocalAxisAdd);
        if (AudioManager.Instance && !_unlockSFX.IsNull)
        {
            AudioManager.Instance.Play(_unlockSFX, loop: false, follow: gameObject);
        }
    }
}
