using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellHandler : MonoBehaviour {

    public event Action? SafeAreaCompleted;

    [SerializeField] private GameObject playerCell = null;
    [SerializeField] private GameObject[] enemyCellPrefabs = null;
    [SerializeField] int dockedPartsToCompleteSafeArea = 0;

    public List<Cell> LivingCells = new List<Cell>();

    public Cell PlayerCell { get; private set; } = null;

    private void Awake() {
        FindAnyObjectByType<CellPartHandler>().CellPartAttached += OnCellPartAttached;
    }

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

    public void OnCellPartAttached(CellPart? part) {

        Cell cell = CellQ.CellOf(part!)!;

        if (CellQ.IsPlayerCell(cell)) {

            int amount = CellQ.IterAllPartsIn(cell).Count();

            if (amount >= dockedPartsToCompleteSafeArea) {
                SafeAreaCompleted?.Invoke();
            }

        }

    }

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