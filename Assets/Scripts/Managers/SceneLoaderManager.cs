using System;
using System.Collections;
using Core.Audio;
using Core.Singletons;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Managers
{
    public class SceneLoaderManager : MonoBehaviourSingletonDontDestroyOnLoad<SceneLoaderManager>
    {
        [Header("===== ANIMATION =====")]
        [SerializeField] public Material _blackScreenGm;
        [Range(0, 1)] public float _valueBlackEffect;
        [Range(0,1)] public float _valueSmoothEffect = 1f;

        [Header("===== SFX =====")] 
        [SerializeField] private EventReference _menuMusic;
        private EventInstance _menuMusicInstance;
    
        private string _sceneName;

        private void Start()
        {
            if (AudioManager.Instance && !_menuMusic.IsNull)
            {
                _menuMusicInstance = AudioManager.Instance.Play(_menuMusic, loop: true);
            }
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
                StartCoroutine(LoadSceneRoutine());
            });
        }
    
        [ContextMenu("Disable")]
        private void DisableBlackScreen()
        {
            DOTween.To(() => _valueBlackEffect, x => _valueSmoothEffect = x, 1, 1.5f);
            DOTween.To(() => _valueBlackEffect, x => _valueBlackEffect = x, 1, 1.5f);
        }

        private IEnumerator LoadSceneRoutine()
        {
            if (AudioManager.Instance && _menuMusicInstance.isValid())
            {
                AudioManager.Instance.Stop(_menuMusicInstance, STOP_MODE.ALLOWFADEOUT);
                _menuMusicInstance = default;
            }
            SceneManager.LoadScene(_sceneName);
            yield return new WaitForSeconds(1f);
            DisableBlackScreen();   
        }
    
    }
}