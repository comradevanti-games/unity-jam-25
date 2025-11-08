using UnityEngine;

public class ForwardMovementMotor : MonoBehaviour, IMotor
{
    [SerializeField] private float maxMovementForce;
    [SerializeField] private float turnForce;
    [SerializeField] private float energyBurnRate;

    private new Rigidbody rigidbody = null!;
    private EnergyHandler energyHandler = null!;

    public void MoveIn(Vector3 direction)
    {
        var scale = Vector3.Dot(transform.forward, direction);
        if (scale < 0.75) return;

        var force = maxMovementForce * scale;
        rigidbody.AddForce(transform.forward * force, ForceMode.Force);

        var burnedEnergy = force * energyBurnRate * Time.fixedDeltaTime;
        energyHandler.OnEnergyBurned(burnedEnergy);
    }

    public void TurnIn(float direction)
    {
        rigidbody.AddTorque(Vector3.up * (direction * turnForce),
            ForceMode.Force);

        var burnedEnergy = turnForce * Time.fixedDeltaTime;
        energyHandler.OnEnergyBurned(burnedEnergy);
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        energyHandler = FindAnyObjectByType<EnergyHandler>();
    }
}