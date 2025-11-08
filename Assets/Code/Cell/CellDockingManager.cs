using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public record DockingIncident(
    Cell CellA,
    CellPart PartA,
    Cell CellB,
    CellPart PartB);

public record DockEventArg(CellPart Parent, CellPart Child);

public class CellDockingManager : MonoBehaviour
{
    public UnityEvent<DockEventArg> partDocked = new UnityEvent<DockEventArg>();

    private readonly ISet<DockingIncident> unresolvedIncidents =
        new HashSet<DockingIncident>();


    private void TryDock(CellPart parent, CellPart child)
    {
        Debug.Assert(parent.Children);
        Debug.Assert(child.Parent);

        child.Parent!.Part = parent;
        parent.Children!.Dock(child);

        partDocked.Invoke(new DockEventArg(parent, child));
    }

    private void Resolve(DockingIncident incident)
    {
        // Cells can't dock with themselves
        if (CellQ.IsSameCell(incident.CellA, incident.CellB)) return;

        switch (incident.PartA, incident.PartB)
        {
            case ({Children: not null}, {Parent: not null}):
                TryDock(incident.PartA, incident.PartB);
                break;
            case ({Parent: not null}, {Children: not null}):
                TryDock(incident.PartB, incident.PartA);
                break;
        }
    }

    private void Update()
    {
        foreach (var incident in unresolvedIncidents) Resolve(incident);
        unresolvedIncidents.Clear();
    }

    private bool HasIncidentBetween(Cell a, Cell b) =>
        unresolvedIncidents.Any(it =>
            (it.CellA == a && it.CellB == b) ||
            (it.CellA == b && it.CellB == a));

    public void ReportDockingIncident(DockingIncident incident)
    {
        // Check whether we are already aware of this incident
        if (HasIncidentBetween(incident.CellA, incident.CellB)) return;

        unresolvedIncidents.Add(incident);
    }
}