using System.Collections;
using Core.Singletons;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviourSingleton<SceneLoaderManager>
{
    // _ApertureSize
    [Range(0, 1)] public float _valueBlackEffect;
    [Range(0,1)] public float _valueSmoothEffect = 1f;
    [SerializeField] public Material _blackScreenGm;
    [SerializeField] private bool _sceneLoaded = false;
    
    private string _sceneName;
    
    public static SceneLoaderManager instance;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
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
        DOTween.To(() => _valueBlackEffect, x => _valueBlackEffect = x, 0, 0.5f);
        DOTween.To(() => _valueSmoothEffect, x => _valueSmoothEffect = x, 0, 1.5f).OnComplete(() =>
        {
            StartCoroutine("TimeToLoad");
        });
    }
    
    [ContextMenu("Disable")]
    private void DisableBlackScreen()
    {
        //z_blackScreenGm.SetFloat("_ApertureSize", 1);
        DOTween.To(() => _valueSmoothEffect, x => _valueSmoothEffect = x, 1, 1.5f);
        DOTween.To(() => _valueBlackEffect, x => _valueBlackEffect = x, 1, 1.5f);
    }

    private IEnumerator TimeToLoad()
    {
        SceneManager.LoadScene(_sceneName);
        yield return new WaitForSeconds(1f);
        DisableBlackScreen();   
    }
    
}
