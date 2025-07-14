using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PresentCellMenu : BaseCellMenu<PresentCell, PresentCell_SO>
{
    public override void PerformAction()
    {
        int cellIdOnIsland = currentCell.posInArray;
        int islandId = currentCell.islandData.islandPosInArray;
        Cell_SO emptyCell = cellData_SO.emptyCells[Random.Range(0, cellData_SO.emptyCells.Length)];
        GameData.instance.IncreaseFlask(Random.Range(currentCellSO.minValue, currentCellSO.maxValue));
        CoinCollectionAnimManager.instance.InitCollectCoinAnim(currentCell.gameObject.transform.position, ValutaData.ValutaType.Flask);
        IslandBuilding.Instance.GetIslands()[islandId].CellChanger(emptyCell, cellIdOnIsland);
        CloseMenu();
    }
}
