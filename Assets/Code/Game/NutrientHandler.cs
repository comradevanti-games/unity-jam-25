using System;
using System.Collections.Generic;
using UnityEngine;

public class NutrientHandler : MonoBehaviour {

    [SerializeField] private int maxAmount = 0;
    [SerializeField] private GameObject smallNutrientPrefab = null;

    private List<GameObject> currentNutrients = new List<GameObject>();

    private void Start() {

        World world = FindAnyObjectByType<World>();

        for (int i = 0; i <= maxAmount; i++) {
            currentNutrients.Add(Instantiate(smallNutrientPrefab, world.GetRandomWorldPoint(), Quaternion.identity));
        }

    }

}