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

        [Header("Positions Clock")] 
        [SerializeField] private Vector2 _positionWhenOption;
        [SerializeField] private Vector2 _positionWhenCredits;
        [SerializeField] private Vector2 _positionWhenExit;

        [Header("Position")] 
        [SerializeField] private Transform _positionOption;
        [SerializeField] private Transform _positionCredits;
        [SerializeField] private Transform _positionExit;
        
        [SerializeField] private GameObject _aiguille;
        [SerializeField] private Transform _selectedPosition;

        private RectTransform _rectTransform;
        private GameObject _currentPanel;
        private bool _canClick = true;
        
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
            if (!_canClick) return;
            _optionPanel.SetActive(true);
            _creditsPanel.SetActive(false);
            _exitPanel.SetActive(false);
            
            SetDefaultDir(_positionOption);
            if(VerifyIfItsSame(_optionPanel)) return;
            
            MoveClockPanelToPosition(_positionWhenOption);
        }

        public void Credits()
        {
            if (!_canClick) return;
            _creditsPanel.SetActive(true);
            _optionPanel.SetActive(false);
            _exitPanel.SetActive(false);
            
            SetDefaultDir(_positionCredits);
            if(VerifyIfItsSame(_creditsPanel)) return;
            
            MoveClockPanelToPosition(_positionWhenCredits);
        }

        public void Quit()
        {
            if (!_canClick) return;
            _exitPanel.SetActive(true);
            _optionPanel.SetActive(false);
            _creditsPanel.SetActive(false);
            
            SetDefaultDir(_positionExit);
            if(VerifyIfItsSame(_exitPanel)) return;
            
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
            ).SetUpdate(true);

            DOTween.To(() => _rectTransform.offsetMin, x => _rectTransform.offsetMin = x, 
                new Vector2(0, position.y), 0.5f
            ).SetUpdate(true).OnComplete(() =>
            {
                _canClick = true;
            });
        }

        public void OnButtonSelected(Vector3 position)
        {
            Vector3 globalDir = position - _aiguille.transform.position;
            Vector3 localDir = _aiguille.transform.parent.InverseTransformDirection(globalDir);
            
            float angle = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg;
            _aiguille.transform.DOLocalRotate(new Vector3(0, 0, angle), 0.3f).SetUpdate(true);
        }

        public void OnButtonDeselected()
        {
            Vector3 dir = _selectedPosition.position - _aiguille.transform.localPosition;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            _aiguille.transform.DOLocalRotate(new Vector3(0, 0, angle), 0.3f).SetUpdate(true);
        }

        private void SetDefaultDir(Transform t)
        {
            _canClick = false;
            _selectedPosition.position = t.localPosition;
        }

        private bool VerifyIfItsSame(GameObject panel)
        {
            if (panel == _currentPanel)
            {
                panel.GetComponent<AnimationPanel>().ClosePanel();
                MoveClockPanelToPosition(new Vector2(0, 0));
                _currentPanel = null;
                return true;
            }
            else
            {
                _currentPanel = panel;
                return false;
            }
        }
    }
}
