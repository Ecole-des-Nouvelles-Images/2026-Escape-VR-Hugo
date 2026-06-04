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
        [SerializeField] private GameObject _spawner;

        public bool GameInPause;
    
        public void PauseGame()
        {
            if (_menu) _menu.SetActive(true);
            GameInPause = true;
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            _menu.SetActive(false);
            _exitPanel.SetActive(false);
            _optionPanel.SetActive(false);
            GameInPause = false;
            _spawner.SetActive(false);
            Time.timeScale = 1f;
        }

        public void SpawnTeleport()
        {
            _spawner.SetActive(true);
        }
    }
}
