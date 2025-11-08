using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public event Action<Vector2>? MovementInputHandled;
    public event Action<float>? TurnInput;

    private bool gameInputLocked = true;

    private void Awake()
    {
        FindAnyObjectByType<GameHandler>().GameInitialized += OnGameInitialized;
    }

    private void OnGameInitialized(bool isInitialized)
    {
        gameInputLocked = !isInitialized;
    }

    public void OnMovementInputReceived(InputAction.CallbackContext ctx)
    {
        if (gameInputLocked) return;

        if (ctx.performed)
            MovementInputHandled?.Invoke(ctx.ReadValue<Vector2>());

        if (ctx.canceled) MovementInputHandled?.Invoke(Vector2.zero);
    }

    public void OnTurnInputReceived(InputAction.CallbackContext ctx)
    {
        if (gameInputLocked) return;

        if (ctx.performed) TurnInput?.Invoke(ctx.ReadValue<float>());
        else if (ctx.canceled) TurnInput?.Invoke(0);
    }
}