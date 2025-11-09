using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnType {

    Nutrient,
    Cell,
    Enemy

}

public class EnergyHandler : MonoBehaviour {

    [SerializeField] private EnergyAreaSo safeArea = null;
    [SerializeField] private EnergyAreaSo fullArea = null;

    private CellPartHandler cellPartHandler = null;
    private NutrientHandler nutrientHandler = null;
    private World world = null;

    private EnergyAreaSo currentArea = null;

    private float StoredWorldEnergy { get; set; }

    private SpawnType QueuedSpawnType { get; set; }

    private float CurrentWorldEnergy => cellPartHandler.CellPartsAmount * currentArea.defaultCellPartEnergy +
                                        nutrientHandler.NutrientAmount * currentArea.defaultNutrientPartEnergy;

    private void Awake() {
        cellPartHandler = FindAnyObjectByType<CellPartHandler>();
        nutrientHandler = FindAnyObjectByType<NutrientHandler>();
        world = FindAnyObjectByType<World>();
        world.SafeAreaCompleted += OnSafeAreaCompleted;
        currentArea = safeArea;
    }

    public void InitializeWorldEnergy() {
        StoredWorldEnergy = currentArea.maxEnergyAmount;
        float playerEnergy = 90;
        UseWorldEnergy(playerEnergy);
        float cellPartEnergy = currentArea.initialCellParts * currentArea.defaultCellPartEnergy;

        if (StoredWorldEnergy - cellPartEnergy < 0) {
            Debug.Log("Too much energy used on cell parts! Reduce initial amount.");
        }

        cellPartHandler.SpawnCellPart(currentArea.initialCellParts);
        UseWorldEnergy(cellPartEnergy);

        nutrientHandler.SpawnNutrient((int)(StoredWorldEnergy / currentArea.defaultNutrientPartEnergy));
        UseWorldEnergy(StoredWorldEnergy);

        GetNextSpawnType();
    }

    public void OnSafeAreaCompleted() {

        cellPartHandler.ResetAll();
        nutrientHandler.ResetAll();
        currentArea = fullArea;
        Camera.main!.GetComponent<CameraFollow>().SetCameraDistance(15f);
        InitializeWorldEnergy();

    }

    private void UseWorldEnergy(float usedEnergy) {
        StoredWorldEnergy -= usedEnergy;
    }

    public void ReturnEnergy(float amount) {
        StoredWorldEnergy += amount;

        if (CurrentWorldEnergy >= currentArea.maxEnergyAmount) return;

        if (QueuedSpawnType == SpawnType.Nutrient && StoredWorldEnergy >= currentArea.defaultNutrientPartEnergy) {
            ReleaseStoredEnergy(QueuedSpawnType);

            return;
        }

        if (QueuedSpawnType == SpawnType.Cell && StoredWorldEnergy >= currentArea.defaultCellPartEnergy) {
            ReleaseStoredEnergy(QueuedSpawnType);
        }

    }

    private void ReleaseStoredEnergy(SpawnType toSpawn) {

        if (toSpawn == SpawnType.Nutrient) {
            nutrientHandler.SpawnNutrient(1);
            UseWorldEnergy(currentArea.defaultCellPartEnergy);
        }

        if (toSpawn == SpawnType.Cell) {
            cellPartHandler.SpawnCellPart(1);
        }

        GetNextSpawnType();

    }

    private void GetNextSpawnType() {

        float percentage = Random.Range(0f, 1f);

        if (percentage < currentArea.enemyAppearChance) {
            //TODO: Spawn Enemy
        }

        if (percentage < currentArea.cellPartAppearChance) {
            QueuedSpawnType = SpawnType.Cell;

            return;
        }

        QueuedSpawnType = SpawnType.Nutrient;
    }

}