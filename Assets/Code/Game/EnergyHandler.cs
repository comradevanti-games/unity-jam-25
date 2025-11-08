using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnType {

    Nutrient,
    Cell,
    Enemy

}

public class EnergyHandler : MonoBehaviour {

    [SerializeField] private float maxEnergyAmount = 500f;
    [SerializeField] private float initialCellPartPercentage = 5f;
    [SerializeField] private float defaultCellPartEnergy = 15f;
    [SerializeField] private float defaultNutrientPartEnergy = 2f;
    [SerializeField] private float cellPartAppearChance = 0.2f;
    [SerializeField] private float enemyAppearChance = 0.1f;

    private CellPartHandler cellPartHandler = null;
    private NutrientHandler nutrientHandler = null;

    public float StoredWorldEnergy { get; private set; }

    private SpawnType QueuedSpawnType { get; set; }

    private float CurrentWorldEnergy => cellPartHandler.CellPartsAmount * defaultCellPartEnergy +
                                        nutrientHandler.NutrientAmount * defaultNutrientPartEnergy;

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
        QueuedSpawnType = SpawnType.Nutrient;
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

        if (CurrentWorldEnergy >= maxEnergyAmount) return;

        if (QueuedSpawnType == SpawnType.Nutrient && StoredWorldEnergy >= defaultNutrientPartEnergy) {
            ReleaseStoredEnergy(QueuedSpawnType);
        }

        if (QueuedSpawnType == SpawnType.Cell && StoredWorldEnergy >= defaultCellPartEnergy) {
            ReleaseStoredEnergy(QueuedSpawnType);
        }

    }

    private void ReleaseStoredEnergy(SpawnType toSpawn) {

        if (toSpawn == SpawnType.Nutrient) {
            nutrientHandler.SpawnNutrient(1);
            UseWorldEnergy(defaultCellPartEnergy);
        }

        if (toSpawn == SpawnType.Cell) {
            cellPartHandler.SpawnCellPart(1);
        }

        GetNextSpawnType();
        Debug.Log("Released Stored Energy!");
        Debug.Log("Rerolled to next: " + QueuedSpawnType);

    }

    private void GetNextSpawnType() {

        float percentage = Random.Range(0f, 1f);

        if (percentage < enemyAppearChance) {
            //TODO: Spawn Enemy
        }

        if (percentage < cellPartAppearChance) {
            QueuedSpawnType = SpawnType.Cell;
        }

        QueuedSpawnType = SpawnType.Nutrient;
    }

}