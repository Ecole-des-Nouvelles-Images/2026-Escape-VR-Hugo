using Core.Audio;
using FMODUnity;
using Managers;
using UnityEngine;

namespace MonoBehiavors
{
    public class SceneLoader : MonoBehaviour
    {
        public string SceneName;
        [SerializeField] private float _maxTime;
        [SerializeField] private float _currentTime;
        [SerializeField] private bool _isOnSocle;
        [SerializeField] private bool _loadMenu;
        //[SerializeField] private UnityEvent EventWhenLoad;

        [Header("==== FMOD AUDIO ====")] 
        [SerializeField] private EventReference _clockDongSFX;

        [SerializeField] private Player _player;
        private bool _isLoading = false;

        private void Start()
        {
            _player = Player.Instance.GetComponent<Player>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!_player) _player = other.gameObject.GetComponent<Player>();
                Debug.Log("Player Enter");
                _isOnSocle = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isOnSocle = false;
                _currentTime = 0;
            }
        }

        private void Update()
        {
            if (_isLoading) return;

            if (!_isOnSocle)
            {
                _currentTime = 0;
                return;
            }

            _currentTime += Time.deltaTime;

            if (_currentTime >= _maxTime)
            {
                _isLoading = true;
                if (!_loadMenu) _player.DisableUiRay();else {_player.EnableUiRay();}
            
                Debug.Log("Load Scene : " + SceneName);
                
                if (AudioManager.Instance && !_clockDongSFX.IsNull)
                {
                    AudioManager.Instance.Play(_clockDongSFX, loop: false, follow: gameObject);
                }
            
                SceneLoaderManager.Instance.LoadScene(SceneName);
                Time.timeScale = 1f;
            }
        }
    }
}
