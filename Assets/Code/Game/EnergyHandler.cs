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
    private World world = null;

    public float StoredWorldEnergy { get; private set; }

    private SpawnType QueuedSpawnType { get; set; }

    private float CurrentWorldEnergy => cellPartHandler.CellPartsAmount * defaultCellPartEnergy +
                                        nutrientHandler.NutrientAmount * defaultNutrientPartEnergy;

    private void Awake() {
        cellPartHandler = FindAnyObjectByType<CellPartHandler>();
        nutrientHandler = FindAnyObjectByType<NutrientHandler>();
        world = FindAnyObjectByType<World>();
        world.SafeAreaCompleted += OnSafeAreaCompleted;
    }

    public void InitializeWorldEnergy() {
        StoredWorldEnergy = maxEnergyAmount;
        float cellPartEnergy = StoredWorldEnergy * initialCellPartPercentage;
        int cellPartAmount = (int)(cellPartEnergy / defaultCellPartEnergy);
        cellPartHandler.SpawnCellPart(cellPartAmount);
        UseWorldEnergy(cellPartEnergy);
        nutrientHandler.SpawnNutrient((int)(StoredWorldEnergy / defaultNutrientPartEnergy));
        UseWorldEnergy(StoredWorldEnergy);
        GetNextSpawnType();
    }

    public void OnSafeAreaCompleted() {
        Debug.Log("Resetting World Energy!");

        cellPartHandler.ResetAll();
        nutrientHandler.ResetAll();

    }

    private void UseWorldEnergy(float usedEnergy) {

        if (StoredWorldEnergy - usedEnergy >= 0) {
            StoredWorldEnergy -= usedEnergy;
        }

    }

    public void ReturnEnergy(float amount) {
        StoredWorldEnergy += amount;

        if (CurrentWorldEnergy >= maxEnergyAmount) return;

        if (QueuedSpawnType == SpawnType.Nutrient && StoredWorldEnergy >= defaultNutrientPartEnergy) {
            ReleaseStoredEnergy(QueuedSpawnType);
            return;
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

    }

    private void GetNextSpawnType() {

        float percentage = Random.Range(0f, 1f);

        if (percentage < enemyAppearChance) {
            //TODO: Spawn Enemy
        }

        if (percentage < cellPartAppearChance) {
            QueuedSpawnType = SpawnType.Cell;
            return;
        }

        QueuedSpawnType = SpawnType.Nutrient;
    }

}