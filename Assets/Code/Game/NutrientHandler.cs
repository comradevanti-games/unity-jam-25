using System;
using System.Collections.Generic;
using UnityEngine;

public class NutrientHandler : MonoBehaviour {

    public event Action<float> NutrientConsumed;

    [SerializeField] private int maxAmount = 0;
    [SerializeField] private GameObject smallNutrientPrefab = null;

    private List<Nutrient> currentNutrients = new();

    private void Start() {

        World world = FindAnyObjectByType<World>();

        for (int i = 0; i <= maxAmount; i++) {
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

}