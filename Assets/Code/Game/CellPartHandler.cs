using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellPartHandler : MonoBehaviour {

    public event Action<CellPart>? CellPartAttached;

    [SerializeField] private GameObject[] availableCellParts = null;

    private List<GameObject> currentCellParts = new List<GameObject>();

    public int CellPartsAmount => currentCellParts.Count;

    public void SpawnCellPart(int amount) {
        var world = FindAnyObjectByType<World>();

        for (var i = 0; i <= amount; i++) {
            var c = Instantiate(
                availableCellParts[Random.Range(0, availableCellParts.Length)],
                world.GetRandomWorldPoint(), Quaternion.identity);
            currentCellParts.Add(c);
        }
    }

    public void OnCellPartAttached(DockEventArg e) {
        if (currentCellParts.Contains(e.Child.gameObject)) {
            currentCellParts.Remove(e.Child.gameObject);
            CellPartAttached?.Invoke(e.Child);
        }

    }

    public void ResetAll() {

        foreach (GameObject n in currentCellParts) {
            Destroy(n);
        }

        currentCellParts = new();
    }

}