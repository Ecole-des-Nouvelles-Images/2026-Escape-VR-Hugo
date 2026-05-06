using _Branches.Hugo.Scripts.Temporal;
using UnityEngine;

namespace _Branches.Hugo.Scripts.PuzzleFlower
{
    public class StemBouquetHandler : TemporalGameObject
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        
        protected override void TimeBehavior()
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(0, _state * 100);
        }
    }
}