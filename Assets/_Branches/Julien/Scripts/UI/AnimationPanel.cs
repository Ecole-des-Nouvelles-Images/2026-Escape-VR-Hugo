using DG.Tweening;
using UnityEngine;

public class AnimationPanel : MonoBehaviour
{
    private void OnEnable()
    {
        Vector3 scale = transform.localScale;
        scale.y = 0;
        transform.localScale = scale;
        
        transform.DOScaleY(1, 0.5f);
    }
}
