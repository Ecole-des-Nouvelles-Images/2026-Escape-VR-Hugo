using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string _sceneNameToLoad;

    [SerializeField] private GameObject _optionPanel;
    [SerializeField] private GameObject _exitPanel;
    

    public void Options()
    {
        _optionPanel.SetActive(true);
        _exitPanel.SetActive(false);
    }

    public void Quit()
    {
        _exitPanel.SetActive(true);
        _optionPanel.SetActive(false);
    }

    public void ReturnMainMenu()
    {
        SceneLoaderManager.Instance.LoadScene(_sceneNameToLoad);
    }
}
