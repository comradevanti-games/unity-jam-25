using System;
using System.Collections.Generic;
using UnityEngine;

public class NutrientHandler : MonoBehaviour {

    public event Action<float> NutrientConsumed;

    [SerializeField] private GameObject smallNutrientPrefab = null;

    private List<Nutrient> currentNutrients = new();

    public int NutrientAmount => currentNutrients.Count;

    public void SpawnNutrient(int amount) {

        World world = FindAnyObjectByType<World>();

        for (int i = 0; i < amount; i++) {
            Nutrient n = Instantiate(smallNutrientPrefab, world.GetRandomWorldPoint(), Quaternion.identity)
                .GetComponent<Nutrient>();
            n.Consumed += OnNutrientConsumed;
            currentNutrients.Add(n);
        }

    }

    private void OnNutrientConsumed(Nutrient consumedNutrient) {
        NutrientConsumed?.Invoke(consumedNutrient.energy);
        consumedNutrient.Consumed -= OnNutrientConsumed;
        currentNutrients.Remove(consumedNutrient);
        Destroy(consumedNutrient.gameObject);
    }

    public void ResetAll() {

        foreach (Nutrient n in currentNutrients) {
            n.Consumed -= OnNutrientConsumed;
            Destroy(n.gameObject);
        }

        currentNutrients = new();
    }

}