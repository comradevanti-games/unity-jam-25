using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CellChildren))]
public class CellBrain : MonoBehaviour
{
    private CellChildren children = null!;

    public void ApplyMovement(Vector3 direction)
    {
        var motors = children.Parts.Select(part => part.GameObject)
            .SelectNotNull(g => g.GetComponent<IMotor>());

        foreach (var motor in motors) motor.MoveIn(direction);
    }

    private void Awake()
    {
        children = GetComponent<CellChildren>();
    }
}