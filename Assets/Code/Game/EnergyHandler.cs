using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnergyHandler : MonoBehaviour {

    [SerializeField] private float totalEnergyAmount = 1000f;
    [SerializeField] private int initialCellParts = 5;
    [SerializeField] private float cellPartAppearChance = 0.2f;
    [SerializeField] private float storedEnergyReleaseThreshold = 10f;

    private CellPartHandler cellPartHandler = null;
    private NutrientHandler nutrientHandler = null;

    public float StoredWorldEnergy { get; private set; }

    private void Awake() {
        cellPartHandler = FindAnyObjectByType<CellPartHandler>();
        nutrientHandler = FindAnyObjectByType<NutrientHandler>();
    }

    public void InitializeWorldEnergy() {
        nutrientHandler.SpawnNutrient((int)(totalEnergyAmount / 2f));
        cellPartHandler.SpawnCellPart(8);
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

        if (percentage < cellPartAppearChance) {
            cellPartHandler.SpawnCellPart(1);
        }

        nutrientHandler.SpawnNutrient(5);
        StoredWorldEnergy = 0;

    }

}