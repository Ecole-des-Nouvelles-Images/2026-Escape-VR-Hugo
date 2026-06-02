using System;
using Core.Audio;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("===== SLIDERS =====")] 
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _gameplaySlider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (AudioManager.Instance)
        {
            InitializeSliders();
        }
    }

    void InitializeSliders()
    {
        if (_masterSlider) _masterSlider.value = AudioManager.Instance.MasterVolume;
        if (_musicSlider) _musicSlider.value = AudioManager.Instance.MusicVolume;
        if (_gameplaySlider) _gameplaySlider.value = AudioManager.Instance.GameplayVolume;
        
        if (_masterSlider) _masterSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetMasterVolume(value));
        if (_musicSlider) _musicSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetMusicVolume(value));
        if (_gameplaySlider) _gameplaySlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetGameplayVolume(value));
    }

    private void OnDestroy()
    {
        if (_masterSlider) _masterSlider.onValueChanged.RemoveAllListeners();
        if (_musicSlider) _musicSlider.onValueChanged.RemoveAllListeners();
        if (_gameplaySlider) _gameplaySlider.onValueChanged.RemoveAllListeners();
    }
}
