using System.Collections.Generic;
using UnityEngine;

namespace MonoBehiavors
{
    public class UtilityTransform : MonoBehaviour
    {
        public Transform Target;
        public List<Transform> Children = new();

        [ContextMenu("SetTransform")]
        public void SetTransform()
        {
            foreach (var child in Children)
            {
                child.transform.position = Target.position;
                child.transform.rotation = Target.rotation;
            }
        }
    }
}