using System;
using System.Collections.Generic;
using UnityEngine;

public class CellHandler : MonoBehaviour {

    [SerializeField] private GameObject playerCell = null;
    [SerializeField] private GameObject[] enemyCellPrefabs = null;

    public List<Cell> LivingCells = new List<Cell>();

    public Cell PlayerCell { get; private set; } = null;

    public void OnCellDeath(Cell dyingCell) {

        if (CellQ.IsPlayerCell(dyingCell)) {
            LivingCells.Remove(dyingCell);
            SpawnPlayerCell(new Vector3(-60, 1, -55));
        }

        LivingCells.Remove(dyingCell);

    }

    public void SpawnPlayerCell(Vector3 position) {
        Cell? cell = SpawnCell(playerCell, position);
        Camera.main!.GetComponent<CameraFollow>().SetFollowTarget(cell!.Root.transform);
        PlayerCell = cell;
    }

    public void OnCellPartAttached() { }

    public Cell? SpawnCell(GameObject cellToSpawn, Vector3 position) {

        GameObject cellGameObject = Instantiate(cellToSpawn, position, Quaternion.identity);
        Cell? cell = UnzipCell(cellGameObject);

        if (cell == null) return null;

        cell.Root.gameObject.GetComponent<CellEnergyStore>().died.AddListener(OnCellDeath);
        LivingCells.Add(cell);
        return cell;

    }

    private Cell? UnzipCell(GameObject cellPrefab) {

        Cell? c = CellQ.CellOf(cellPrefab.GetComponentInChildren<CellBrain>().gameObject);

        if (c == null) return null;

        cellPrefab.transform.DetachChildren();

        Destroy(cellPrefab);
        return c;

    }

}