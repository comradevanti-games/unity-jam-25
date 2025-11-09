using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Nutrient : MonoBehaviour {

    public event Action<Nutrient>? Consumed;

    public float energy = 2f;

    [SerializeField] private GameObject[] possibleNutrientForms = null;
    [SerializeField] private float minAngularSpeed = 10f;
    [SerializeField] private float maxAngularSpeed = 50f;
    [SerializeField] private float minRotationDuration = 2f;
    [SerializeField] private float maxRotationDuration = 5f;

    private Rigidbody rb;
    private float rotationTimer;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {

        int randomNutrient = Random.Range(0, possibleNutrientForms.Length);

        for (int i = 0; i < possibleNutrientForms.Length; i++) {
            possibleNutrientForms[i].SetActive(i == randomNutrient);
        }

        PickNewRotation();

    }

    private void Update() {

        rotationTimer -= Time.deltaTime;

        if (rotationTimer <= 0f) {
            PickNewRotation();
        }

    }

    private void PickNewRotation() {
        Vector3 randomAxis = Random.onUnitSphere;
        float randomSpeed = Random.Range(minAngularSpeed, maxAngularSpeed);
        rotationTimer = Random.Range(minRotationDuration, maxRotationDuration);
        rb.angularVelocity = randomAxis * randomSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        // Only cell-parts which are part of a cell can consume nutrients
        if (other.GetComponentInParent<CellPart>() is not { } part) return;
        if (!CellQ.IsPartOfCell(part)) return;
        
        Consumed?.Invoke(this);
    }

}