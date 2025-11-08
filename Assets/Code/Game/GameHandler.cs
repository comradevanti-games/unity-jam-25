using System;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public event Action<bool>? GameInitialized;

    private EnergyHandler energyHandler = null;

    private void Awake() {
        Application.targetFrameRate = 60;
        energyHandler = FindAnyObjectByType<EnergyHandler>();
    }

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        energyHandler.InitializeWorldEnergy();
        GameInitialized?.Invoke(true);
    }

}