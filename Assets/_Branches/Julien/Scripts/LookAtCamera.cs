using UnityEngine;

namespace _Branches.Julien.Scripts
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _target;

        private void Start()
        {
            if (Camera.main != null)
            {
                _target = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (!_target) return;
        
            Vector3 targetDirection = transform.position - _target.position;

            if (targetDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(targetDirection);
            }
        }
    }
}