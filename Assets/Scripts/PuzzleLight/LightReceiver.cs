using System;
using Core.Interfaces;
using UnityEngine;

public class LightReceiver : MonoBehaviour, ILightReactive
{
    public event Action OnLit; 

    public void IsLit()
    {
        OnLit?.Invoke();
    }
}
