using System;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    [SerializeField] private float _maxTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private bool _isOnSocle;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("LoadScene");
            _isOnSocle = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isOnSocle = false;
    }

    private void Update()
    {
        if (!_isOnSocle)
        {
            _currentTime = 0;
            return;
        }
        else
        {
            _currentTime += Time.deltaTime;
        }
        
        if (_currentTime >= _maxTime)
        {
            SceneLoaderManager.Instance.LoadScene(SceneName);
        }
    }
}
