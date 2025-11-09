using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CellPart : MonoBehaviour
{
    public UnityEvent<CellPart?> dockChanged = new UnityEvent<CellPart?>();

    [SerializeField] private bool isDock;
    [SerializeField] private float energyContent;

    private CellDockingManager cellDockingManager = null!;
    private FixedJoint? dockJoint;
    private new Rigidbody rigidbody = null!;
    private CellPart? dock;
    private readonly ISet<CellPart> docked = new HashSet<CellPart>();

    /// <summary>
    /// How much energy is stored in this part
    /// </summary>
    public float EnergyContent => energyContent;

    /// <summary>
    /// Whether this part can dock onto other parts
    /// </summary>
    public bool CanDock => dockJoint;

    /// <summary>
    /// Whether other parts can dock onto this one.
    /// </summary>
    public bool IsDock => isDock;

    public CellPart? Dock
    {
        get => dock;
        set
        {
            if (!CanDock) return;
            if (value != null && !value.IsDock) return;

            if (dock != null)
                dock.docked.Remove(this);

            dock = value;
            dockJoint!.connectedBody = value?.rigidbody;

            if (dock != null)
                dock.docked.Add(this);

            dockChanged.Invoke(value);
        }
    }

    /// <summary>
    /// Whether this part is currently docked onto some other part.
    /// </summary>
    public bool IsDocked => dock != null;

    public IEnumerable<CellPart> Docked => docked;

    private void Awake()
    {
        dockJoint = GetComponent<FixedJoint>();
        rigidbody = GetComponent<Rigidbody>();
        cellDockingManager = FindAnyObjectByType<CellDockingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // We only initiate docking, if we are a dock and are part of a cell
        if (!IsDock) return;

        var selfCell = CellQ.CellOf(this);
        if (selfCell == null) return;
        
        // Check if we collided with a cell
        var otherPart = CellQ.TryAsCellPart(other.gameObject);
        if (otherPart == null) return;

        // If the other part is already part of some larger structure then
        // we can't dock with it.
        if (otherPart.IsDocked) return;

        var incident = new DockingIncident(selfCell, this, otherPart);
        cellDockingManager.ReportDockingIncident(incident);
    }

    private void OnDestroy()
    {
        FindAnyObjectByType<EnergyHandler>()?.ReturnEnergy(energyContent);
    }
}