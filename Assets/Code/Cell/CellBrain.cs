using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CellPart))]
public class CellBrain : MonoBehaviour
{
    private CellPart part = null!;


    private IEnumerable<IMotor> Motors =>
        CellQ.IterDockedPartsRecursive(part)
            .SelectNotNull(it => it.GetComponent<IMotor>());

    public void ApplyMovement(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        foreach (var motor in Motors) motor.MoveIn(direction);
    }

    public void ApplyTurn(float direction)
    {
        if (direction == 0) return;
        foreach (var motor in Motors) motor.TurnIn(direction);
    }

    private void Awake()
    {
        part = GetComponent<CellPart>();
    }
}