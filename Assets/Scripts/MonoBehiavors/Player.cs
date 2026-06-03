using Core.Singletons;
using UI;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

namespace MonoBehiavors
{
    public class Player : MonoBehaviourSingletonDontDestroyOnLoad<Player>
    {
        [SerializeField] private GameObject _interactorRayLeft;
        [SerializeField] private GameObject _interactorRayRight;
        [SerializeField] private ContinuousMoveProvider _moveProvider;
        [SerializeField] private ContinuousTurnProvider _turnProvider;
        
        [SerializeField] private bool _useJoysticks;
        public InputActionProperty InputOpenMenu;
        
        // [Header("DebugMenu")]
        // public InputActionProperty InputDebugMenu;
        // [SerializeField] private bool _debugMenuOpen;
        // [SerializeField] private GameObject _debugMenu;

        protected override void Awake()
        {
            base.Awake();
            _moveProvider = GetComponentInChildren<ContinuousMoveProvider>();
            _turnProvider = GetComponentInChildren<ContinuousTurnProvider>();
        }

        private void OnEnable()
        {
            InputOpenMenu.action.performed += OnClickPause;
            //InputDebugMenu.action.performed += DebugMenu;
        }
        private void OnDisable()
        {
            InputOpenMenu.action.performed -= OnClickPause;
            //InputDebugMenu.action.performed -= DebugMenu;
        }

        public void DisableUiRay()
        {
            _interactorRayLeft.SetActive(false);
            _interactorRayRight.SetActive(false);
        }

        public void EnableUiRay()
        {
            _interactorRayLeft.SetActive(true);
            _interactorRayRight.SetActive(true);
        }

        [ContextMenu("Pause")]
        private void OnClickPause(InputAction.CallbackContext obj)
        {
            Debug.Log("Pause");
            if (!PauseMenu.Instance.GameInPause)
            {
                PauseMenu.Instance.PauseGame();
                EnableUiRay();
            }
            else
            {
                PauseMenu.Instance.Resume();
                DisableUiRay();
            }
        }
    
        [ContextMenu("Pause")]
        private void DebugClickPauseMenu()
        {
            if (!PauseMenu.Instance.GameInPause)
            {
                PauseMenu.Instance.PauseGame();
                EnableUiRay();
            }
            else
            {
                PauseMenu.Instance.Resume();
                DisableUiRay();
            }
        }

        public void SetLocomotion()
        {
            Debug.Log("Set locomotion");
            if (!_useJoysticks)
            {
                _moveProvider.enabled = true;
                _turnProvider.enabled = true;
                _useJoysticks = true; 
            }
            else
            {
                _moveProvider.enabled = false;
                _turnProvider.enabled = false;
                _useJoysticks = false;
            }
        }
        
        // debug
        // private void DebugMenu(InputAction.CallbackContext obj)
        // {
        //     if (!_debugMenuOpen)
        //     {
        //         _debugMenuOpen = true;
        //         _debugMenu.SetActive(true);
        //         EnableUiRay();
        //     }
        //     else
        //     {
        //         _debugMenuOpen = false;
        //         _debugMenu.SetActive(false);
        //         if (!PauseMenu.Instance.GameInPause) DisableUiRay();
        //     }
        // }
        
    }
}
