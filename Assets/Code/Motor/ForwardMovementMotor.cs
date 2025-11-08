using UnityEngine;

public class ForwardMovementMotor : MonoBehaviour, IMotor
{
    [SerializeField] private float maxMovementForce;
    [SerializeField] private float turnForce;

    private new Rigidbody rigidbody = null!;

    public void MoveIn(Vector3 direction)
    {
        var scale = Vector3.Dot(transform.forward, direction);
        if (scale < 0.75) return;

        var force = maxMovementForce * scale;
        rigidbody.AddForce(transform.forward * force, ForceMode.Force);
    }

    public void TurnIn(float direction)
    {
        rigidbody.AddTorque(Vector3.up * (direction * turnForce),
            ForceMode.Force);
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
}