using System;
using Core.Audio;
using Core.Interfaces;
using FMODUnity;
using UnityEngine;

namespace PuzzleLight
{
    public class MechanismLightReceiver : MonoBehaviour, ILightReactive
    {
        public event Action<bool> OnLit;
        
        [Header("===== FMOD AUDIO =====")]
        [SerializeField] private EventReference _beamIgniteSFX;

        public void OnLightEnter()
        {
            if (AudioManager.Instance && !_beamIgniteSFX.IsNull)
            {
                AudioManager.Instance.PlayAtPosition(_beamIgniteSFX, transform.position, loop: false);
            }
            OnLit?.Invoke(true);
        }

        public void OnLightExit()
        {
            OnLit?.Invoke(false);
        }
    }
}
