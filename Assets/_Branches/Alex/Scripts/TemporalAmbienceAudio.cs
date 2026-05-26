using Core.Audio;
using FMOD.Studio;
using FMODUnity;
using Managers;
using MonoBehiavors;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class TemporalAmbienceSound : TemporalGameObject
{
    [Header("===== FMOD AMBIENCE =====")]
    [SerializeField] private EventReference _ambienceEvent;
    [SerializeField] private string _timeParameterName = "TimeOfDay";
    
    private EventInstance _ambienceInstance;
    private bool _isMutedByClock;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (AudioManager.Instance)
        {
           StartAmbience();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ClockTimeManager.Instance) return;
        bool isManipulatingTime = ClockTimeManager.Instance.IsPaused;
        if (isManipulatingTime && !_isMutedByClock)
        {
            if (_ambienceInstance.isValid())
            {
                _ambienceInstance.setVolume(0f);
            }
            _isMutedByClock = true;
        }
        else if (!isManipulatingTime && _isMutedByClock)
        {
            if (_ambienceInstance.isValid())
            {
                _ambienceInstance.setVolume(1f);
                _ambienceInstance.setParameterByName(_timeParameterName, _state);
            }
            _isMutedByClock = false;
        }

    }

    protected override void TimeBehavior()
    {
        if (_ambienceInstance.isValid())
        {
            _ambienceInstance.setParameterByName(_timeParameterName, _state);
        }
    }

    void StartAmbience()
    {
        if (!_ambienceInstance.isValid())
        {
            _ambienceInstance = AudioManager.Instance.Play(_ambienceEvent, loop: true, follow: gameObject);
            _ambienceInstance.setParameterByName(_timeParameterName, _state);
        }
    }
    
    private void StopAmbience(STOP_MODE mode)
    {
        if (_ambienceInstance.isValid())
        {
            _ambienceInstance.stop(mode);
            _ambienceInstance.release();
            _ambienceInstance = new EventInstance();
        }
    }

    private void OnDestroy()
    {
        if (_ambienceInstance.isValid())
        {
            _ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _ambienceInstance.release();
        }
    }
}
