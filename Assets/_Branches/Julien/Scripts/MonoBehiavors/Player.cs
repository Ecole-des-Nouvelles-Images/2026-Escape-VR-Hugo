using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _interactorRayLeft;
    [SerializeField] private GameObject _interactorRayRight;
    
    public static Player instance;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
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
        PauseMenu.Instance.PauseGame();
        EnableUiRay();
    }

    [ContextMenu("Unpause game")]
    public void DisablePauseMenu()
    {
        PauseMenu.Instance.Resume();
        DisableUiRay();
    }
}
