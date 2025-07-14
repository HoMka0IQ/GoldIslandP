using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellData_SO", menuName = "Cells/New Cells Data")]
public class CellData_SO : ScriptableObject
{
    public Cell_SO[] allCells;

    public Cell_SO[] allCommonCells;
    public float rareChance;
    public Cell_SO[] allRareCells;
    public Cell_SO[] allEpicCells;

    public Cell_SO[] emptyCells;

    public BuildCell_So[] buildingCells;
    public PresentCell_SO[] presentCells;
}
