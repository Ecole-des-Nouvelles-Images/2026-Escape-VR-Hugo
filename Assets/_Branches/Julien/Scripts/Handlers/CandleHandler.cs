using _Branches.Hugo.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CandleHandler : TemporalGameObject
{
    [Header("==== Candle ====")] 
    
    [Header("Start")]
    [SerializeField] private bool _setValueInStart;
    [SerializeField] private Vector2 _fireStarteTime;
    
    [Header("Visual")]
    
    [SerializeField] private GameObject _candleVisual;

    [Header("Fire")]
    [SerializeField] private GameObject _flameGameObject;
    [SerializeField] private bool _isFire;
    
    [Header("ObjectInside")] 
    
    [SerializeField] private float _valueDrop;
    [SerializeField] private GameObject _objectToDrop;
    [SerializeField] private bool _dropedHisObject;

    private void Start()
    {
        if (_setValueInStart)
        {
            _temporalRange.x = _fireStarteTime.x; 
            _temporalRange.y = _fireStarteTime.y; 
        }
    }

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (_isFire) return;
        _flameGameObject.SetActive(true);
        _isFire = true;
        if (!_setValueInStart)_temporalRange.x = ClockTimeManager.Instance.NormalizedCurrentTime;
        _temporalRange.y = _temporalRange.x + 0.3f;
        Debug.Log("Fire");
    }
    
    protected override void TimeBehavior()
    {
        _candleVisual.transform.localScale = new Vector3(1, 1 - _state, 1);
        if (_state >= _valueDrop && !_dropedHisObject)
        {
            DropObjectInCandle();
        }

        if (ClockTimeManager.Instance.NormalizedCurrentTime > _temporalRange.x && _state < _temporalRange.y && !_isFire)
        {
            Fire();
        }
        
        if (ClockTimeManager.Instance.NormalizedCurrentTime < _temporalRange.x && _isFire)
        {
            _temporalRange = new Vector2(0, 0);
            BlowOut();
        }
    }

    private void BlowOut()
    {
        _flameGameObject.SetActive(false);
        _isFire = false;
    }
    
    private void DropObjectInCandle()
    {
        if (_objectToDrop == null) return;
        _objectToDrop.transform.parent = null;
        _objectToDrop.GetComponent<XRGrabInteractable>().enabled = true;
        _objectToDrop.GetComponent<BoxCollider>().isTrigger = false;
        _objectToDrop.GetComponent<Rigidbody>().isKinematic = false;
        _dropedHisObject = true;
        Debug.Log("Dropped object in candle");
    }
}
