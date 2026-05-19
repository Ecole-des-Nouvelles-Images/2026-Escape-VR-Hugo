using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string _sceneNameToLoad;

        [SerializeField] private GameObject _optionPanel;
        [SerializeField] private GameObject _creditsPanel;
        [SerializeField] private GameObject _exitPanel;

        [Header("POsitions Clock")] 
        [SerializeField] private Vector2 _positionWhenOption;
        [SerializeField] private Vector2 _positionWhenCredits;
        [SerializeField] private Vector2 _positionWhenExit;

        [SerializeField] private GameObject _aiguille;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Play()
        {
            SceneManager.LoadScene(_sceneNameToLoad);
            Debug.Log("Loading scene");
        }

        public void Options()
        {
            _optionPanel.SetActive(true);
            _creditsPanel.SetActive(false);
            _exitPanel.SetActive(false);
            MoveClockPanelToPosition(_positionWhenOption);
        }

        public void Credits()
        {
            _creditsPanel.SetActive(true);
            _optionPanel.SetActive(false);
            _exitPanel.SetActive(false);
            MoveClockPanelToPosition(_positionWhenCredits);
        }

        public void Quit()
        {
            _exitPanel.SetActive(true);
            _optionPanel.SetActive(false);
            _creditsPanel.SetActive(false);
            MoveClockPanelToPosition(_positionWhenExit);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
        [ContextMenu("Test")]
        public void MoveClockPanelToPosition(Vector2 position)
        {
            DOTween.To(() => _rectTransform.offsetMax, x => _rectTransform.offsetMax = x,
                new Vector2(0, position.x), 0.5f
            );

            DOTween.To(() => _rectTransform.offsetMin, x => _rectTransform.offsetMin = x, 
                new Vector2(0, position.y), 0.5f
            );
        }

        public void OnButtonSelected(Vector3 position)
        {
            Debug.Log(position);
            _aiguille.transform.DOLookAt(position, 0.3f);
        }
    }
}
