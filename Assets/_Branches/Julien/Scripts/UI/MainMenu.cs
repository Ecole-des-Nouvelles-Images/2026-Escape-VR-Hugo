using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneNameToLoad;

    [SerializeField] private GameObject _optionPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _exitPanel;
    
    public void Play()
    {
        SceneManager.LoadScene(_sceneNameToLoad);
        Debug.Log("Loading scene");
    }

    public void Options()
    {
        Debug.Log("Option");
        _optionPanel.SetActive(true);
        _creditsPanel.SetActive(false);
        _exitPanel.SetActive(false);
    }

    public void Credits()
    {
        Debug.Log("Credits");
        _creditsPanel.SetActive(true);
        _optionPanel.SetActive(false);
        _exitPanel.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        _exitPanel.SetActive(true);
        _optionPanel.SetActive(false);
        _creditsPanel.SetActive(false);
    }
}
