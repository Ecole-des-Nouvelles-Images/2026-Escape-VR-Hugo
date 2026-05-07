using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    [SerializeField] private float _maxTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private bool _isOnSocle;
    
    private bool _isLoading = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

            Debug.Log("Load Scene : " + SceneName);

            SceneLoaderManager.Instance.LoadScene(SceneName);
        }
    }
}
