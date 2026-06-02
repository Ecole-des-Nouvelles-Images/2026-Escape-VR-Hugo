using System;
using Core;
using Core.Audio;
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

        private void Start()
        {
            PlayNarration(_narrationEvent01);
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
        }
        
        private void TriggerNarration02() => PlayNarration(_narrationEvent02);
        private void TriggerNarration03() => PlayNarration(_narrationEvent03);
        private void TriggerNarration04() => PlayNarration(_narrationEvent04);
        
        private void PlayNarration(EventReference sfx)
        {
            if (AudioManager.Instance && !sfx.IsNull)
            {
                AudioManager.Instance.Play(sfx);
            }
        }
    }
}
