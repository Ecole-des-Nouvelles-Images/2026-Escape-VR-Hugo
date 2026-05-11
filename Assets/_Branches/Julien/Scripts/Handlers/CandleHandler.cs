using _Branches.Hugo.Scripts.Temporal;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CandleHandler : TemporalGameObject
{
    [Header("==== Candle ====")] 
    [SerializeField] private GameObject _candleVisual;

    [Header("Fire")]
    [SerializeField] private GameObject _flameGameObject;
    [SerializeField] private bool _isFire;
    [SerializeField] private Transform _firePoint;
    
    [Header("ObjectInside")] 
    [SerializeField] private float _valueDrop;
    [SerializeField] private GameObject _objectToDrop;
    [SerializeField] private bool _dropedHisObject;

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (_isFire) return;
        _isFire = true;
        _flameGameObject.SetActive(true);
        float currentTime = ClockTimeManager.Instance.NormalizedCurrentTime;
        _temporalRange = new Vector2(currentTime, currentTime + 0.3f);
    }
    
    protected override void TimeBehavior()
    {
        _candleVisual.transform.localScale = new Vector3(1, 1 - _state, 1);
        if (_flameGameObject) _flameGameObject.transform.position = _firePoint.position;

        if (_state >= _valueDrop && !_dropedHisObject)
        {
            DropObjectInCandle();
        }

        if (!_isFire) return;

        float currentTime = ClockTimeManager.Instance.NormalizedCurrentTime;

        if (currentTime < _temporalRange.x)
        {
            BlowOut();
        }
        else if (currentTime > _temporalRange.y)
        {
            if (_flameGameObject.activeSelf) _flameGameObject.SetActive(false);
        }
        else
        {
            if (!_flameGameObject.activeSelf) _flameGameObject.SetActive(true);
        }
    }

    private void BlowOut()
    {
        _flameGameObject.SetActive(false);
        _isFire = false; // Bloque le TimeBehavior jusqu'au prochain Fire()
        _temporalRange = Vector2.zero; // Reset pour sécurité
    }

    private void DropObjectInCandle()
    {
        if (_objectToDrop == null) return;
        _objectToDrop.transform.parent = null;
        _objectToDrop.GetComponent<XRGrabInteractable>().enabled = true;
        _objectToDrop.GetComponent<BoxCollider>().isTrigger = false;
        _objectToDrop.GetComponent<Rigidbody>().isKinematic = false;
        _dropedHisObject = true;
    }
}