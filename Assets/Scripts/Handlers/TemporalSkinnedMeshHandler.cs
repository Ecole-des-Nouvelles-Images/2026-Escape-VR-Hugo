using System.Collections.Generic;
using MonoBehiavors;
using UnityEngine;

namespace Handlers
{
    public class TemporalSkinnedMeshHandler : TemporalGameObject
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private List<int> _blendShapeIndexes = new();
        
        private Material[] _materials;

        private void Start()
        {
            _materials = _skinnedMeshRenderer.materials;
        }

        protected override void TimeBehavior()
        {
            if (!_skinnedMeshRenderer || _blendShapeIndexes.Count == 0) return;
            
            foreach (var blendShapeIndex in _blendShapeIndexes)
            {
                _skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, _state * 100);
            }

            if (_materials.Length > 0)
            {
                foreach (var material in _materials)
                {
                    material.SetFloat("_TimeFane", _state);
                }
            }
        }
    }
}