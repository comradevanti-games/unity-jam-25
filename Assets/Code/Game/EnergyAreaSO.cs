using UnityEngine;

// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnergyArea", order = 1)]
public class EnergyAreaSo : ScriptableObject {

    public float maxEnergyAmount;
    public float initialCellPartPercentage;
    public float defaultCellPartEnergy;
    public float defaultNutrientPartEnergy;
    public float cellPartAppearChance;
    public float enemyAppearChance;

}