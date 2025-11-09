using System;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public event Action<bool>? GameInitialized;

    [SerializeField] private GameObject playerCell = null;

    private EnergyHandler energyHandler = null;

    private CellHandler cellHandler = null;

    private void Awake() {
        Application.targetFrameRate = 60;
        cellHandler = FindAnyObjectByType<CellHandler>();
        energyHandler = FindAnyObjectByType<EnergyHandler>();
    }

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        Transform? playerT = cellHandler.SpawnCell(playerCell, new Vector3(-60, 1, -55));
        Camera.main!.GetComponent<CameraFollow>().SetFollowTarget(playerT!);
        energyHandler.InitializeWorldEnergy();
        GameInitialized?.Invoke(true);
    }

}