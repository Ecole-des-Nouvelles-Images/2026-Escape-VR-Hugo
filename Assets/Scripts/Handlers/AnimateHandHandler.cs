using UnityEngine;
using UnityEngine.InputSystem;

namespace Handlers
{
    public class AnimateHandHandler : MonoBehaviour
    {
        public InputActionProperty TriggerValue;
        public InputActionProperty GripValue;

        public Animator HandAnimator;

        private void Update()
        {
            float trigger =  TriggerValue.action.ReadValue<float>();
            float grip = GripValue.action.ReadValue<float>();
        
            HandAnimator.SetFloat("Trigger", trigger);
            HandAnimator.SetFloat("Grip", grip);
        }
    }
}
