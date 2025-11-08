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

    private void OnMovementInput(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void OnTurnInput(float direction)
    {
        turnDirection = direction;
    }

    private void Awake()
    {
        brain = GetComponent<CellBrain>();
        FindAnyObjectByType<RelativeDirectionalInput>()
            .inputReceived.AddListener(OnMovementInput);
        FindAnyObjectByType<InputHandler>().TurnInput += OnTurnInput;
    }
}