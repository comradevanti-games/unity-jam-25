using UnityEngine;

public class InputDrivenCellController : MonoBehaviour
{
    private CellBrain brain = null!;

    private Vector3 moveDirection = Vector3.zero;

    private void FixedUpdate()
    {
        if (moveDirection == Vector3.zero) return;
        brain.ApplyMovement(moveDirection);
    }

    private void OnMovementInput(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void Awake()
    {
        brain = GetComponent<CellBrain>();
        FindAnyObjectByType<RelativeDirectionalInput>()
            .inputReceived.AddListener(OnMovementInput);
    }
}