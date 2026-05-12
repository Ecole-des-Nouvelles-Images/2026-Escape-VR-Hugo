using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class VRHeadFade : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float _fadeDuration = 0.2f;
    
    private int _overlapCount = 0;
    private float _currentAlpha = 0f; // Variable locale pour suivre l'état de l'alpha

    private void Start()
    {
        _currentAlpha = 0f;
        _material.SetFloat("_Alpha", 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        _overlapCount++;
        StartFade(1f);
    }

    private void OnTriggerExit(Collider other)
    {
        _overlapCount = 0;
        StartFade(0f);
    }

    private void StartFade(float targetValue)
    {
        _material.DOKill();
        DOTween.To(() => _currentAlpha, x => { _currentAlpha = x; _material.SetFloat("_Alpha", _currentAlpha); }
                , targetValue, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true); 
    }

    private void OnDestroy()
    {
        _material.SetFloat("_Alpha", 0f);
    }
}