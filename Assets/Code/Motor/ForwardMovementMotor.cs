using UnityEngine;

public class ForwardMovementMotor : MonoBehaviour, IMotor
{
    [SerializeField] private float maxMovementForce;

    private new Rigidbody rigidbody = null!;

    public void MoveIn(Vector3 direction)
    {
        var scale = Vector3.Dot(transform.forward, direction);
        if (scale < 0) return;

        var force = maxMovementForce * scale;
        rigidbody.AddForce(direction * force, ForceMode.Force);
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
}