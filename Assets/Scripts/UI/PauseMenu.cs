using Core.Singletons;
using Managers;
using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviourSingleton<PauseMenu>
    {
        [SerializeField] private string _sceneNameToLoad;

        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _optionPanel;
        [SerializeField] private GameObject _exitPanel;

        public bool GameInPause;
    
        public void PauseGame()
        {
            _menu.SetActive(true);
            GameInPause = true;
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            _menu.SetActive(false);
            GameInPause = false;
            Time.timeScale = 1f;
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
            Time.timeScale = 1f;
            SceneLoaderManager.Instance.LoadScene(_sceneNameToLoad);
        }
    }
}
