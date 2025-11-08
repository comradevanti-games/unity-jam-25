using System.Collections.Generic;
using UnityEngine;

public record Cell(CellPart Root);

public static class CellQ
{
    public static bool IsPlayerCell(Cell cell) =>
        cell.Root.CompareTag("Player");

    public static IEnumerable<CellPart> IterDockedPartsRecursive(CellPart part)
    {
        yield return part;

        foreach (var directDocked in part.Docked)
        foreach (var docked in IterDockedPartsRecursive(directDocked))
            yield return docked;
    }

    public static IEnumerable<CellPart> IterAllPartsIn(Cell cell) =>
        IterDockedPartsRecursive(cell.Root);

    public static float EnergyOf(Cell cell) =>
        cell.Root.GetComponent<CellEnergyStore>().Energy;

    public static CellPart? TryAsCellPart(GameObject go) =>
        go.GetComponent<CellPart>();

    private static Cell? TryAsCell(CellPart part) =>
        part.gameObject.GetComponent<CellBrain>()
            ? new Cell(part)
            : null;

    public static Cell? CellOf(CellPart part)
    {
        // If we are not docked then we check if we are a cell root
        if (part.Dock is not { } dock) return TryAsCell(part);

        // Otherwise search for the cell in the dock
        return CellOf(dock);
    }

    public static Cell? CellOf(GameObject go)
    {
        var part = TryAsCellPart(go);
        if (part is null) return null;
        return CellOf(part);
    }

    public static bool IsSameCell(Cell a, Cell b) => a.Root == b.Root;
}