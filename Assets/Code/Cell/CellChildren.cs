using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a reference to attached cell children.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CellChildren : MonoBehaviour
{
    private readonly ISet<CellPart> parts = new HashSet<CellPart>();

    public IEnumerable<CellPart> Parts => parts;

    public void Dock(CellPart child)
    {
        parts.Add(child);
    }
}