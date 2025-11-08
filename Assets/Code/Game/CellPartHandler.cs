using System.Collections.Generic;
using UnityEngine;

public class CellPartHandler : MonoBehaviour {

    [SerializeField] private GameObject[] availableCellParts = null;

    private List<GameObject> currentCellParts = new();

    public void SpawnCellPart(int amount) {

        World world = FindAnyObjectByType<World>();

        for (int i = 0; i <= amount; i++) {
            currentCellParts.Add(Instantiate(availableCellParts[Random.Range(0, availableCellParts.Length)],
                world.GetRandomWorldPoint(), Quaternion.identity));
        }

    }

}