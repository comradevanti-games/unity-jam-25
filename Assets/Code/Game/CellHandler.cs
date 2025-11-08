using System;
using System.Collections.Generic;
using UnityEngine;

public class CellHandler : MonoBehaviour {

    [SerializeField] private GameObject playerCell = null;
    [SerializeField] private GameObject[] enemyCellPrefabs = null;

    public List<Cell> LivingCells = new List<Cell>();

    public void OnCellDeath(Cell dyingCell) {

        if (CellQ.IsPlayerCell(dyingCell)) {
            LivingCells.Remove(dyingCell);
            SpawnCell(playerCell, new Vector3(0, 1, 0));
        }

        LivingCells.Remove(dyingCell);

    }

    public Transform? SpawnCell(GameObject cellToSpawn, Vector3 position) {

        GameObject cellGameObject = Instantiate(cellToSpawn, position, Quaternion.identity);
        Cell? cell = UnzipCell(cellGameObject);

        if (cell != null) {
            cell.Root.gameObject.GetComponent<CellEnergyStore>().died.AddListener(OnCellDeath);
            LivingCells.Add(cell);

            return cell.Root.gameObject.transform;
        }

        return null;

    }

    private Cell? UnzipCell(GameObject cellPrefab) {

        Cell? c = CellQ.CellOf(cellPrefab.GetComponentInChildren<CellBrain>().gameObject);

        if (c == null) return null;

        cellPrefab.transform.DetachChildren();

        Destroy(cellPrefab);

        return c;

    }

}