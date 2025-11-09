using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CellEnergyStore : MonoBehaviour {
    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");

    public UnityEvent<Cell> died = new UnityEvent<Cell>();

    [SerializeField] private float initialEnergy;
    [SerializeField] private Renderer brainRenderer = null!;

    private EnergyHandler energyHandler = null!;
    private float energy;
    private Color fullEnergyColor;


    private float FullEnergy => initialEnergy * 2;
    
    public float Energy
    {
        get => energy;
        private set
        {
            if (Mathf.Approximately(value, energy)) return;
            energy = value;

            var energyT = Mathf.InverseLerp(0, FullEnergy, energy);
            var color = Color.Lerp(Color.black, fullEnergyColor, energyT);
            brainRenderer.material.SetColor(baseColor, color);

            if (Energy == 0) Die();
        }
    }

    private bool IsDead => Energy == 0;

    private void Die() {
        var cell = CellQ.CellOf(gameObject)!;
        var parts = CellQ.IterAllPartsIn(cell).ToArray();
        foreach (var part in parts) Destroy(part.gameObject);
        died?.Invoke(cell);
    }

    public void Gain(float amount)
    {
        if (IsDead) return;
        
        Energy += amount;
    }

    public void Burn(float amount)
    {
        if (IsDead) return;
        
        amount = Mathf.Min(Energy, amount);
        energyHandler.ReturnEnergy(amount);
        Energy -= amount;
    }

    private void Awake() {
        fullEnergyColor = brainRenderer.material.GetColor(baseColor);
        Energy = initialEnergy;
        energyHandler = FindAnyObjectByType<EnergyHandler>();
    }

}