using System.Collections;
using Core.Singletons;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviourSingleton<SceneLoaderManager>
{
    // _ApertureSize
    [Range(0,1)] public float _valueBlackEffect = 1;
    [Range(0,1)] public float _valueSmoothEffect = 0.8f;
    [SerializeField] public Material _blackScreenGm;
    [SerializeField] private bool _sceneLoaded = false;
    
    private string _sceneName;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadScene(string sceneName)
    {
        Debug.Log("LoadedScene");
        _sceneName = sceneName;
        EnableBlackScreen();
    }

    private void Update()
    {
        _blackScreenGm.SetFloat("_ApertureSize", _valueBlackEffect);
        _blackScreenGm.SetFloat("_FeatheringEffect", _valueSmoothEffect);
    }

    [ContextMenu("Enable")]
    public void EnableBlackScreen()
    {
        DOTween.To(() => _valueBlackEffect, x => _valueBlackEffect = x, 0, 1f).OnComplete(() =>
        {
            DOTween.To(() => _valueSmoothEffect, x => _valueSmoothEffect = x, 0, 1).OnComplete(() =>
            {
                StartCoroutine("TimeToLoad");
            });
        });
    }
    
    [ContextMenu("Disable")]
    private void DisableBlackScreen()
    {
        DOTween.To(() => _valueBlackEffect, x => _valueBlackEffect = x, 1, 1f).OnComplete(() =>
        {
            DOTween.To(() => _valueSmoothEffect, x => _valueSmoothEffect = x, 1, 1f);
        });
    }

    private IEnumerator TimeToLoad()
    {
        SceneManager.LoadScene(_sceneName);
        yield return new WaitForSeconds(1f);
        DisableBlackScreen();   
    }
    
}
