using _Branches.Hugo.Scripts.Temporal;
using UnityEngine;

namespace _Branches.Hugo.Scripts.PuzzleFlower
{
    public class PetalsBouquetHandler : TemporalGameObject
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        
        protected override void TimeBehavior()
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(1, _state * 100);
        }
    }
}