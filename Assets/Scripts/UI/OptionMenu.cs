using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionMenu : MonoBehaviour
    {
        [SerializeField] private Toggle _toggleMoveJoystick;

        private void OnEnable()
        {
            _toggleMoveJoystick.isOn = MonoBehiavors.Player.Instance.UseJoysticks;
        }

        public void SetLocomotionToPlayer()
        {
            MonoBehiavors.Player.Instance.SetLocomotion();
        }
    }
}
