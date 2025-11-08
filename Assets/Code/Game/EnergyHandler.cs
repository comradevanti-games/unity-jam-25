using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnergyHandler : MonoBehaviour {

    [SerializeField] private float maxEnergyAmount = 500f;
    [SerializeField] private float initialCellPartPercentage = 5f;
    [SerializeField] private float defaultCellPartEnergy = 15f;
    [SerializeField] private float defaultNutrientPartEnergy = 2f;
    [SerializeField] private float cellPartAppearChance = 0.2f;
    [SerializeField] private float enemyAppearChance = 0.1f;
    [SerializeField] private float storedEnergyReleaseThreshold = 10f;

    private CellPartHandler cellPartHandler = null;
    private NutrientHandler nutrientHandler = null;

    public float StoredWorldEnergy { get; private set; }

    private void Awake() {
        cellPartHandler = FindAnyObjectByType<CellPartHandler>();
        nutrientHandler = FindAnyObjectByType<NutrientHandler>();
    }

    public void InitializeWorldEnergy() {

        StoredWorldEnergy = maxEnergyAmount;

        float cellPartEnergy = StoredWorldEnergy * initialCellPartPercentage;
        int cellPartAmount = (int)(cellPartEnergy / defaultCellPartEnergy);
        //TODO: Replace defaultCellPartEnergy with energy that is needed to create certain cell parts.
        cellPartHandler.SpawnCellPart(cellPartAmount);
        UseWorldEnergy(cellPartEnergy);
        nutrientHandler.SpawnNutrient((int)(StoredWorldEnergy / defaultNutrientPartEnergy));
        UseWorldEnergy(StoredWorldEnergy);
    }

    private void UseWorldEnergy(float usedEnergy) {

        if (StoredWorldEnergy - usedEnergy >= 0) {
            StoredWorldEnergy -= usedEnergy;
        }
        else {
            Debug.Log("No Energy available!");
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