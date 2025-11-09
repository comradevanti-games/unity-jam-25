using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CellPart))]
public class ForwardMovementMotor : MonoBehaviour, IMotor
{
    [SerializeField] private float maxMovementForce;
    [SerializeField] private float turnForce;
    [SerializeField] private float energyBurnRate;

    private CellEnergyStore? energyStore;
    private new Rigidbody rigidbody = null!;
    private Animator animator = null!;

    private bool isMoving;

    public void MoveIn(Vector3 direction)
    {
        var scale = Vector3.Dot(transform.forward, direction);
        if (scale < 0.5) return;

        var force = maxMovementForce * scale;
        rigidbody.AddForce(transform.forward * force, ForceMode.Force);

        var burnedEnergy = force * energyBurnRate * Time.fixedDeltaTime;
        energyStore?.Burn(burnedEnergy);

        isMoving = true;
    }

    public void TurnIn(float direction)
    {
        rigidbody.AddTorque(Vector3.up * (direction * turnForce),
            ForceMode.Force);

        var burnedEnergy = turnForce * Time.fixedDeltaTime * 0.5f;
        energyStore?.Burn(burnedEnergy);

        isMoving = true;
    }

    private void Update()
    {
        animator.speed = isMoving ? 1 : 0.1f;
    }

    private void OnDockChanged(CellPart? dock)
    {
        if (dock == null)
        {
            energyStore = null;
            return;
        }

        var cell = CellQ.CellOf(dock!);
        Debug.Assert(cell != null);

        energyStore = cell!.Root.GetComponent<CellEnergyStore>();
    }

    private void LateUpdate()
    {
        isMoving = false;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        GetComponent<CellPart>().dockChanged.AddListener(OnDockChanged);
    }
}