using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CellBrain))]
public class AICellController : MonoBehaviour
{
    private CellBrain brain = null!;

    private Vector3 moveDirection;
    private float turnDirection;

    private void ChooseRandomInputs()
    {
        moveDirection =
            new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))
                .normalized;

        turnDirection = Random.Range(-1, 1f);
    }

    private void FixedUpdate()
    {
        if (Random.Range(0, 1f) < 0.1f)
            ChooseRandomInputs();

        brain.ApplyMovement(moveDirection);
        brain.ApplyTurn(turnDirection);
    }

    private void Start()
    {
        ChooseRandomInputs();
    }

    private void Awake()
    {
        brain = GetComponent<CellBrain>();
    }
}