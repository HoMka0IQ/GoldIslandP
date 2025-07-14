using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell_SO", menuName = "Cells/New Cell")]
public class Cell_SO : ScriptableObject
{
    public string cellName;
    public string title;
    public GameObject cellPrefab;
    public bool randomRotationOnSpawn;
    public CellType cellTypes;

    public enum CellType
    {
        None,
        Empty,
        Forest,
        Rocks,
        Factory,
        Specific,
        Present
    }

}
