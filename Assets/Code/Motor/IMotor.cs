using UnityEngine;

public interface IMotor
{
    public void MoveIn(Vector3 direction);

    public void TurnIn(float direction);
}