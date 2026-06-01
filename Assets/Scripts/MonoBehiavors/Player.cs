using Core.Singletons;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonoBehiavors
{
    public class Player : MonoBehaviourSingletonDontDestroyOnLoad<Player>
    {
        [SerializeField] private GameObject _interactorRayLeft;
        [SerializeField] private GameObject _interactorRayRight;

        public InputActionProperty InputOpenMenu;
        
        // [Header("DebugMenu")]
        // public InputActionProperty InputDebugMenu;
        // [SerializeField] private bool _debugMenuOpen;
        // [SerializeField] private GameObject _debugMenu;
        
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
