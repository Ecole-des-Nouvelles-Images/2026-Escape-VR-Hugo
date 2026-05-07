using System;
using Core.Interfaces;
using UnityEngine;

namespace PuzzleLight
{
    public class MechanismLightReceiver : MonoBehaviour, ILightReactive
    {
        public event Action OnLit; 

        public void IsLit()
        {
            OnLit?.Invoke();
        }
    }
}
