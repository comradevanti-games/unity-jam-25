using UnityEngine;

public class CellEnergyStore : MonoBehaviour
{
    [SerializeField] private float initialEnergy;

    private EnergyHandler energyHandler = null!;

    public float Energy { get; private set; }

    public void Burn(float amount)
    {
        amount = Mathf.Min(Energy, amount);
        Energy -= amount;
        energyHandler.OnEnergyBurned(amount);
    }

    private void Awake()
    {
        Energy = initialEnergy;
        energyHandler = FindAnyObjectByType<EnergyHandler>();
    }
}