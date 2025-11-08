using UnityEngine;

public class InputDrivenCellController : MonoBehaviour
{
    private CellBrain brain = null!;

    private Vector3 moveDirection = Vector3.zero;
    private float turnDirection;

    private void FixedUpdate()
    {
        if (moveDirection != Vector3.zero) brain.ApplyMovement(moveDirection);
        if (turnDirection != 0f) brain.ApplyTurn(turnDirection);
    }

    private void OnMovementInput(Vector2 direction)
    {
        var absolute = new Vector3(direction.x, 0, direction.y);
        moveDirection = transform.InverseTransformDirection(absolute);
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