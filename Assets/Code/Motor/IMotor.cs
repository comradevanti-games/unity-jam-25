using UnityEngine;

public interface IMotor
{
    /// <summary>
    /// Applies lateral force in the given direction. Call this in FixedUpdate.
    /// </summary>
    public void MoveIn(Vector3 direction);

    /// <summary>
    /// Applies angular force in the given direction. Call this in FixedUpdate.
    /// </summary>
    public void TurnIn(float direction);
}