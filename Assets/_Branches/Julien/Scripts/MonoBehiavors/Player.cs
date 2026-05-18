using System;
using Core.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviourSingletonDontDestroyOnLoad<Player>
{
    [SerializeField] private GameObject _interactorRayLeft;
    [SerializeField] private GameObject _interactorRayRight;

    public InputActionProperty InputOpenMenu;

    private void OnEnable()
    {
        InputOpenMenu.action.started += OnClickPause;
    }

    private void OnDisable()
    {
        InputOpenMenu.action.started -= OnClickPause;
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
}
