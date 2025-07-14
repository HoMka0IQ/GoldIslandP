using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NatureCell_SO", menuName = "Cells/New Cell")]
public class NatureCell_SO : Cell_SO
{
    public NaturalCellType naturalCellTypes;

    public enum NaturalCellType
    {
        None,
        Air,
        Forest,
        Rock
    }
}
