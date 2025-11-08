using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour {

    public event Action<Vector2>? MovementInputHandled;

    public void OnMovementInputReceived(InputAction.CallbackContext ctx) {

        if (ctx.performed) {
            MovementInputHandled?.Invoke(ctx.ReadValue<Vector2>());
        }

        if (ctx.canceled) {
            MovementInputHandled?.Invoke(Vector2.zero);
        }

    }

}