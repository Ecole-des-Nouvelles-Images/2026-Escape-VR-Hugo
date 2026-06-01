using DG.Tweening;
using UnityEngine;

public class FlameAnimation : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    private Material _materialInstance;

    private void Awake()
    {
        _materialInstance = rend.material;
    }

    private void OnEnable()
    {
        DOTween.To(
            () => 0.6f, x => _materialInstance.SetFloat("_FlameDissolve", x), 0f, 0.5f
        );
    }

    public void BlowOut()
    {
        DOTween.To(
            () => 0f, x => _materialInstance.SetFloat("_FlameDissolve", x), 0.6f, 0.5f
        ).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
