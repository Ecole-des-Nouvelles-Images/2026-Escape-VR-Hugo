using System;
using _Branches.Hugo.Scripts;
using UnityEngine;

public class CandleHandler : TemporalGameObject
{
    [Header("Candle")] 
    [SerializeField] private bool _fireInStart;
    
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
        if (_fireInStart) Fire();
    }

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (_isFire) return;
        Debug.Log("Fire");
        _flameGameObject.SetActive(true);
        _isFire = true;
        _temporalRange.x = ClockTimeManager.Instance.NormalizedCurrentTime;
        _temporalRange.y = _temporalRange.x + 0.3f;
    }
    
    protected override void TimeBehavior()
    {
        _candleVisual.transform.localScale = new Vector3(1, 1 - _state, 1);
        if (_state >= _valueDrop && !_dropedHisObject)
        {
            DropObjectInCandle();
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
        // Rendre l'objec dans la bougie interactible
        _dropedHisObject = true;
        Debug.Log("Dropped object in candle");
    }
}
