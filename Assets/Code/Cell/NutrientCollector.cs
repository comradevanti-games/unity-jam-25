using UnityEngine;

[RequireComponent(typeof(CellPart))]
public class NutrientCollector : MonoBehaviour
{
    private CellPart part = null!;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Nutrient>() is not
            { } nutrient) return;

        var energyStore = CellQ.GetRootComponent<CellEnergyStore>(part);
        energyStore?.Gain(nutrient.energy);
    }

    private void Awake()
    {
        part = GetComponent<CellPart>();
    }
}