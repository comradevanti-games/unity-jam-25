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
        foreach (var motor in Motors) motor.MoveIn(direction);
    }

    public void ApplyTurn(float direction)
    {
        foreach (var motor in Motors) motor.TurnIn(direction);
    }

    private void Awake()
    {
        part = GetComponent<CellPart>();
    }
}