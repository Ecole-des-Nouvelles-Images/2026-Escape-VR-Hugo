using System;
using Core.Interfaces;
using UnityEngine;

namespace PuzzleLight
{
    public class MechanismLightReceiver : MonoBehaviour, ILightReactive
    {
        public event Action<bool> OnLit; 

        public void OnLightEnter()
        {
            OnLit?.Invoke(true);
        }

        public void OnLightExit()
        {
            OnLit?.Invoke(false);
        }
    }
}
