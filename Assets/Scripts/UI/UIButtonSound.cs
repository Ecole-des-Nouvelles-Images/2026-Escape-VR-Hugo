using System;
using Core.Audio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIButtonSound : MonoBehaviour
    {
        [SerializeField] private EventReference _clickSFX;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (_button)
            {
                _button.onClick.AddListener(PlayClickSound);
            }
        }

        private void OnDisable()
        {
            if (_button)
            {
                _button.onClick.RemoveListener(PlayClickSound);
            }
        }

        void PlayClickSound()
        {
            if (AudioManager.Instance && !_clickSFX.IsNull)
            {
                AudioManager.Instance.Play(_clickSFX, loop: false);
            }
        }
    }
}
