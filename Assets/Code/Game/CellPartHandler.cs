using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellPartHandler : MonoBehaviour {

    [SerializeField] private GameObject[] availableCellParts = null;

    private List<GameObject> currentCellParts = new();

    public void SpawnCellPart(int amount) {

        World world = FindAnyObjectByType<World>();

        for (int i = 0; i <= amount; i++) {
            GameObject c = Instantiate(availableCellParts[Random.Range(0, availableCellParts.Length)],
                world.GetRandomWorldPoint(), Quaternion.identity);
            currentCellParts.Add(c);
        }

    }

    public void OnCellPartAttached(DockEventArg e) {

        if (currentCellParts.Contains(e.Child.GameObject)) {
            currentCellParts.Remove(e.Child.GameObject);
        }

    }

}