using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnergyHandler : MonoBehaviour {

    [SerializeField] private float totalEnergyAmount = 1000f;
    [SerializeField] private float cellPartPercentage = 0.2f;
    [SerializeField] private float storedEnergyReleaseThreshold = 10f;

    private CellPartHandler cellPartHandler = null;
    private NutrientHandler nutrientHandler = null;

    public float StoredWorldEnergy { get; private set; }

    private void Awake() {
        cellPartHandler = FindAnyObjectByType<CellPartHandler>();
        nutrientHandler = FindAnyObjectByType<NutrientHandler>();
    }

    public void InitializeWorldEnergy() {
        nutrientHandler.SpawnNutrient(100);
        cellPartHandler.SpawnCellPart(5);
    }

    public void UseWorldEnergy(float usedEnergy) {

        if (totalEnergyAmount - usedEnergy > 0) {
            totalEnergyAmount -= usedEnergy;
        }

    }

    public void OnEnergyBurned(float burnedEnergy) {
        StoredWorldEnergy += burnedEnergy;

        if (StoredWorldEnergy >= storedEnergyReleaseThreshold) {
            ReleaseStoredEnergy();
        }

    }

    private void ReleaseStoredEnergy() {

        float percentage = Random.Range(0f, 1f);

        if (percentage < cellPartPercentage) {
            cellPartHandler.SpawnCellPart(1);
        }
        else {
            nutrientHandler.SpawnNutrient(5);
        }

        StoredWorldEnergy = 0;

    }

}