using System.Collections.Generic;
using _Branches.Hugo.Scripts.Temporal;
using UnityEngine;

namespace _Branches.Hugo.Scripts.PuzzleFlower
{
    public class TemporalSkinnedMeshHandler : TemporalGameObject
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private List<int> _blendShapeIndexes = new();
        
        protected override void TimeBehavior()
        {
            if (!_skinnedMeshRenderer || _blendShapeIndexes.Count == 0) return;
            
            foreach (var blendShapeIndex in _blendShapeIndexes)
            {
                _skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, _state * 100);
            }
        }
    }
}