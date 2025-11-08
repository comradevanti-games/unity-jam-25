using UnityEngine;

public class InputDrivenCellController : MonoBehaviour
{
    private CellBrain brain = null!;

    private Vector2 moveInput = Vector2.zero;
    private float turnDirection;

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            var local = new Vector3(moveInput.x, 0, moveInput.y);
            var moveDirection = transform.TransformDirection(local);
            brain.ApplyMovement(moveDirection);
        }

        if (turnDirection != 0f) brain.ApplyTurn(turnDirection);
    }

    private void OnMovementInput(Vector2 direction)
    {
        moveInput = direction;
       
    }

    private void OnTurnInput(float direction)
    {
        turnDirection = direction;
    }

    private void Awake()
    {
        brain = GetComponent<CellBrain>();
        var inputHandler = FindAnyObjectByType<InputHandler>();
        inputHandler.TurnInput += OnTurnInput;
        inputHandler.MovementInputHandled += OnMovementInput;
    }
}