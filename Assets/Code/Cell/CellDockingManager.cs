using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public record DockingIncident(Cell Cell, CellPart CellPart, CellPart OtherPart);

public record DockEventArg(CellPart Parent, CellPart Child);

public class CellDockingManager : MonoBehaviour
{
    public UnityEvent<DockEventArg> partDocked = new UnityEvent<DockEventArg>();

    private readonly ISet<DockingIncident> unresolvedIncidents =
        new HashSet<DockingIncident>();


    private void Resolve(DockingIncident incident)
    {
        incident.OtherPart.Dock = incident.CellPart;
        partDocked.Invoke(new DockEventArg(incident.CellPart,
            incident.OtherPart));
    }

    private void Update()
    {
        foreach (var incident in unresolvedIncidents) Resolve(incident);
        unresolvedIncidents.Clear();
    }

    public void ReportDockingIncident(DockingIncident incident)
    {
        unresolvedIncidents.Add(incident);
    }
}