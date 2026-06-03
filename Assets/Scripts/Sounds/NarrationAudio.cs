using System;
using Core;
using Core.Audio;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Sounds
{
    public class NarrationAudio : MonoBehaviour
    {
        [Header("==== FMOD AUDIO ====")]
        [SerializeField] private EventReference _narrationEvent01;
        [SerializeField] private EventReference _narrationEvent02;
        [SerializeField] private EventReference _narrationEvent03;
        [SerializeField] private EventReference _narrationEvent04;

        private EventInstance _eventInstance01;
        private bool _isEvent01Playing;
        private bool _isEvent02Waiting;
        
        private bool _hasTriggered02;
        private bool _hasTriggered03;
        private bool _hasTriggered04;

        private void Start()
        {
            _eventInstance01 = PlayNarrationAndGetInstance(_narrationEvent01);
            _isEvent01Playing = _eventInstance01.isValid();
        }

        private void OnEnable()
        {
            EventBus.OnNarrationEvent02 += TriggerNarration02;
            EventBus.OnNarrationEvent03 += TriggerNarration03;
            EventBus.OnNarrationEvent04 += TriggerNarration04;
        }

        private void OnDisable()
        {
            EventBus.OnNarrationEvent02 -= TriggerNarration02;
            EventBus.OnNarrationEvent03 -= TriggerNarration03;
            EventBus.OnNarrationEvent04 -= TriggerNarration04;

            if (_eventInstance01.isValid())
            {
                _eventInstance01.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _eventInstance01.release();
            }
        }

        private void Update()
        {
            if (_isEvent01Playing)
            {
                _eventInstance01.getPlaybackState(out PLAYBACK_STATE state);
        
                if (state == PLAYBACK_STATE.STOPPED)
                {
                    _isEvent01Playing = false;
                    _eventInstance01.release(); 
            
                    if (_isEvent02Waiting)
                    {
                        _isEvent02Waiting = false;
                        PlayNarration(_narrationEvent02);
                    }
                }
            }
        }

        private void TriggerNarration02()
        {
            if (_hasTriggered02) return;
            _hasTriggered02 = true;

            if (_isEvent01Playing)
            {
                _isEvent02Waiting = true;
                return;
            }
    
            PlayNarration(_narrationEvent02);
        }

        private void TriggerNarration03()
        {
            if (_hasTriggered03) return;
            _hasTriggered03 = true;
            PlayNarration(_narrationEvent03);
        }

        private void TriggerNarration04()
        {
            if (_hasTriggered04) return;
            _hasTriggered04 = true;
            PlayNarration(_narrationEvent04);
        }
        private void PlayNarration(EventReference sfx)
        {
            if (AudioManager.Instance && !sfx.IsNull)
            {
                AudioManager.Instance.Play(sfx);
            }
        }

        private EventInstance PlayNarrationAndGetInstance(EventReference sfx)
        {
            if (sfx.IsNull) return default;

            try
            {
                EventInstance instance = RuntimeManager.CreateInstance(sfx);
                instance.start();
                return instance;
            }
            catch (Exception e)
            {
                Debug.LogError($"[NarrationAudio] Erreur lors du lancement de l'Event 01: {e.Message}");
                return default;
            }
        }
    }
}
