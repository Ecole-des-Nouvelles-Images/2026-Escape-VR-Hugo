using _Branches.Hugo.Scripts;
using UnityEngine;

public class CandleHandler : TemporalGameObject
{
    [SerializeField] private GameObject _candleVisual;

    [Header("ObjectInside")] 
    
    [SerializeField] private float _valueDrop;
    [SerializeField] private GameObject _objectToDrop;
    [SerializeField] private bool _dropedHisObject;

    [ContextMenu("Fire")]
    public void Fire()
    {
        // allumer la mèche
    }
    
    protected override void TimeBehavior()
    {
        Debug.Log(_state);
        _candleVisual.transform.localScale = new Vector3(1, 1 - _state, 1);
        if (_state >= _valueDrop && !_dropedHisObject)
        {
            DropObjectInCandle();
        }
    }
    
    private void DropObjectInCandle()
    {
        // Rendre l'objec dans la bougie interactible
        _dropedHisObject = true;
        Debug.Log("Dropped object in candle");
    }
}
