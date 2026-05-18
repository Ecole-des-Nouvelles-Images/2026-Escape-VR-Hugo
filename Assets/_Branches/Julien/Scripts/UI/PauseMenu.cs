using Core.Singletons;
using UnityEngine;

public class PauseMenu : MonoBehaviourSingleton<PauseMenu>
{
    [SerializeField] private string _sceneNameToLoad;

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _optionPanel;
    [SerializeField] private GameObject _exitPanel;

    public void PauseGame()
    {
        _menu.SetActive(true);
    }

    public void Resume()
    {
        _menu.SetActive(false);
    }
    
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
