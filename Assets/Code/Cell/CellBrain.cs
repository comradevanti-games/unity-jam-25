using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CellChildren))]
public class CellBrain : MonoBehaviour
{
    private CellChildren children = null!;


    private IEnumerable<IMotor> Motors =>
        children.Parts.Select(part => part.GameObject)
            .SelectNotNull(g => g.GetComponent<IMotor>());

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
        children = GetComponent<CellChildren>();
    }
}