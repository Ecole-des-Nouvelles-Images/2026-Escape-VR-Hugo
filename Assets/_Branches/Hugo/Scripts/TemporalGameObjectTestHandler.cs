using UnityEngine;

namespace _Branches.Hugo.Scripts
{
    public class TemporalGameObjectTestHandler : TemporalGameObject
    {
        protected override void TimeBehavior()
        {
            float yPos = Mathf.Lerp(10f, 0f, _state);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
    }
}