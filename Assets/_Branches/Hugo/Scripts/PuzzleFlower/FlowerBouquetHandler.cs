using _Branches.Hugo.Scripts.Temporal;
using UnityEngine;

namespace _Branches.Hugo.Scripts.PuzzleFlower
{
    public class FlowerBouquetHandler : TemporalGameObject
    {
        [Header("===== SETTINGS =====")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        
        [Header("===== REFERENCES =====")]
        [SerializeField] private Transform _flowerBouquetTransform;
        
        protected override void TimeBehavior()
        {
            Vector3 targetScale = Vector3.Lerp(_startScale, _endScale, _state);
            _flowerBouquetTransform.localScale = targetScale;
        }
    }
}