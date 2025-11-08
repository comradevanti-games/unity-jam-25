using System.Collections.Generic;
using UnityEngine;

public record Cell(CellPart Root);

public record CellPart(
    GameObject GameObject,
    CellParent? Parent,
    CellChildren? Children);

public static class CellQ
{
    public static bool IsPlayerCell(Cell cell) =>
        cell.Root.GameObject.CompareTag("Player");

    public static IEnumerable<CellPart> IterSubParts(CellPart part)
    {
        yield return part;

        if (part.Children is not { } children) yield break;

        foreach (var directChild in children.Parts)
        foreach (var child in IterSubParts(directChild))
            yield return child;
    }

    public static IEnumerable<CellPart> IterAllParts(Cell cell) =>
        IterSubParts(cell.Root);

    public static CellPart? CellPartOf(GameObject go)
    {
        var parent = go.GetComponent<CellParent>();
        var children = go.GetComponent<CellChildren>();

        if (parent == null && children == null) return null;

        return new CellPart(go, parent, children);
    }

    public static Cell CellOf(CellPart part)
    {
        // If we could have a parent then we need to check if we are docked
        if (part.Parent is { } parent)
        {
            var parentPart = parent.Part;

            // If we are not attached to anything at the moment then we are
            // the root
            if (parentPart == null) return new Cell(part);

            // Otherwise search for the root in the parent
            return CellOf(parentPart);
        }

        // Otherwise we are the root
        return new Cell(part);
    }

    public static bool IsSameCell(Cell a, Cell b) => a.Root == b.Root;
}