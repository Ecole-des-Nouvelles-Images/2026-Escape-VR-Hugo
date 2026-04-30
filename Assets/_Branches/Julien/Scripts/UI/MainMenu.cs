using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneNameToLoad;
    
    public void Play()
    {
        SceneManager.LoadScene(_sceneNameToLoad);
        Debug.Log("Loading scene");
    }

    public void Options()
    {
        Debug.Log("Option");
    }

    public void Credits()
    {
        Debug.Log("Credits"); 
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
