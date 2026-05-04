using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputActionProperty MoveAction;
    public bool CanMoveWithJoystick;

    private void Update()
    {
        if (!CanMoveWithJoystick) return;
        
        Vector2 dir = MoveAction.action.ReadValue<Vector2>();
        transform.Translate(dir);
    }
}
