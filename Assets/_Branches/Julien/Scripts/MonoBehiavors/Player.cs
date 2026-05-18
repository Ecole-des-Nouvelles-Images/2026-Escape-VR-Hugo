using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _interactorRayLeft;
    [SerializeField] private GameObject _interactorRayRight;
    
    [Header("PauseMenu")]
    [SerializeField] private GameObject _pauseMenu;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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

    [ContextMenu("Pause game")]
    public void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);
        EnableUiRay();
    }

    [ContextMenu("Unpause game")]
    public void DisablePauseMenu()
    {
        _pauseMenu.SetActive(false);
        DisableUiRay();
    }
}
