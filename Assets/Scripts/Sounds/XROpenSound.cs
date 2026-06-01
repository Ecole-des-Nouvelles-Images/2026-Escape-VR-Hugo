using Core.Audio;
using FMODUnity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XROpenSound : MonoBehaviour
{
    [Header("===== FMOD AUDIO =====")]
    [SerializeField] private EventReference _openSFX;

    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.AddListener(PlayOpenSound);
        }
    }

    private void OnDisable()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.RemoveListener(PlayOpenSound);
        }
    }

    private void PlayOpenSound(SelectEnterEventArgs args)
    {
        if (AudioManager.Instance && !_openSFX.IsNull)
        {
            AudioManager.Instance.Play(_openSFX, loop: false, follow: gameObject);
        }
    }
}

