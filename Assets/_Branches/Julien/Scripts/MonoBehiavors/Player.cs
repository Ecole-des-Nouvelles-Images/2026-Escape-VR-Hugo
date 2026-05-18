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


    private void Update()
    {
        float valueInputMenu = InputOpenMenu.action.ReadValue<float>();
        if (valueInputMenu >= 1) OpenPauseMenu();
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
        if (!PauseMenu.Instance.GameInPause) OpenPauseMenu(); else DisablePauseMenu();
    }
    
    public void OpenPauseMenu()
    {
        PauseMenu.Instance.PauseGame();
        EnableUiRay();
    }

    public void DisablePauseMenu()
    {
        PauseMenu.Instance.Resume();
        DisableUiRay();
    }
}
