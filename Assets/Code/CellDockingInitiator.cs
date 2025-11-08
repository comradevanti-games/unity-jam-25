using UnityEngine;
using UnityEngine.Assertions;

public class CellDockingInitiator : MonoBehaviour
{
    private CellDockingManager manager = null!;

    private void OnCollisionEnter(Collision other)
    {
        // Check if we collided with a cell
        var otherPart = CellQ.CellPartOf(other.gameObject);
        if (otherPart == null) return;
        var otherCell = CellQ.CellOf(otherPart);


        var selfPart = CellQ.CellPartOf(gameObject);
        Debug.Assert(selfPart != null, "Self must be cell part");
        var selfCell = CellQ.CellOf(selfPart!);

        var incident =
            new DockingIncident(selfCell, selfPart!, otherCell, otherPart);

        manager.ReportDockingIncident(incident);
    }

    private void Awake()
    {
        manager = FindAnyObjectByType<CellDockingManager>();
    }
}