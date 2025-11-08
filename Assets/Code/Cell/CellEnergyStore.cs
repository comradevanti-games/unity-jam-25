using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CellEnergyStore : MonoBehaviour {

    public UnityEvent<Cell> died = new UnityEvent<Cell>();

    [SerializeField] private float initialEnergy;

    private EnergyHandler energyHandler = null!;

    public float Energy { get; private set; }

    private void Die() {
        var cell = CellQ.CellOf(gameObject)!;
        var parts = CellQ.IterAllPartsIn(cell).ToArray();
        foreach (var part in parts) Destroy(part.gameObject);
        died?.Invoke(cell);
    }

    public void Burn(float amount) {
        amount = Mathf.Min(Energy, amount);
        Energy -= amount;
        energyHandler.ReturnEnergy(amount);

        if (Energy == 0) Die();
    }

    private void Awake() {
        Energy = initialEnergy;
        energyHandler = FindAnyObjectByType<EnergyHandler>();
    }

}