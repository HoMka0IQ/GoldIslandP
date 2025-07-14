using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell_SO", menuName = "Cells/New Present")]
public class PresentCell_SO : Cell_SO
{
    ValutaData.ValutaType valutaData;
    public int minValue = 100;
    public int maxValue = 1000;

}
