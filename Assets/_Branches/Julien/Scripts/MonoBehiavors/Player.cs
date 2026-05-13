using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _interactorRayLeft;
    [SerializeField] private GameObject _interactorRayRight;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void DisableUiRay()
    {
        _interactorRayLeft.SetActive(false);
        _interactorRayRight.SetActive(false);
    }
}
