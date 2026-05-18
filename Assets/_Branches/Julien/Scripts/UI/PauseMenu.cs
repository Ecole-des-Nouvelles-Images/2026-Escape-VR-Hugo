using Core.Singletons;
using UnityEngine;

public class PauseMenu : MonoBehaviourSingleton<PauseMenu>
{
    [SerializeField] private string _sceneNameToLoad;

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _optionPanel;
    [SerializeField] private GameObject _exitPanel;

    public bool GameInPause;

    public void CallPause()
    {
        
    }
    
    public void PauseGame()
    {
        _menu.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void Resume()
    {
        _menu.SetActive(false);
        //Time.timeScale = 1f;
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
