using UnityEngine;

namespace _Branches.Hugo.Scripts.Temporal
{
    public class TemporalGameObjectTestHandler : TemporalGameObject
    {
        protected override void TimeBehavior()
        {
            float yPos = Mathf.Lerp(1f, 0f, _state);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
    }
}