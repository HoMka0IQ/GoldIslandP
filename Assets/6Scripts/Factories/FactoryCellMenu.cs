using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryCellMenu : BaseCellMenu<FactoryCell, BuildCell_So>
{
    public GameObject upgradeBtn;

    public override void OpenMenu(FactoryCell cell, BuildCell_So cellSO, GameObject go)
    {
        base.OpenMenu(cell, cellSO, go);
        if (cell.currentLVL >= currentCellSO.upgrades.Length)
        {
            upgradeBtn.SetActive(false);
        }
        else
        {
            upgradeBtn.SetActive(true);
        }
    }
    public override void PerformAction()
    {
        object obj = GlobalData.instance.CalcCostProgressionByCount(IslandBuilding.Instance.GetCellCountByName(currentCellSO.cellName) - 1, currentCellSO.startCost) / 2;
        string text = string.Format("Do you want to sell this factory for <color=yellow>{0}<sprite=0></color>?", obj);
        ConfirmationDialog.instance.SetDialog(Action, text);
        CloseMenu();
    }

    public void Action()
    {

        int cellIdOnIsland = currentCell.posInArray;
        int islandId = currentCell.islandData.islandPosInArray;
        Cell_SO emptyCell = cellData_SO.emptyCells[UnityEngine.Random.Range(0, cellData_SO.emptyCells.Length)];
        IslandBuilding.Instance.GetIslands()[islandId].CellChanger(emptyCell, cellIdOnIsland);
        BuildZonesManager.instance.HideAll();
        CoinCollectionAnimManager.instance.InitCollectCoinAnim(currentCell.transform.position, ValutaData.ValutaType.Money);
        GameData.instance.IncreaseMoney(GlobalData.instance.CalcCostProgressionByCount(IslandBuilding.Instance.GetCellCountByName(currentCellSO.cellName) - 1, currentCellSO.startCost) / 2);
        
    }

    public void UpgradeFactory()
    {
        currentCell.UpgradeFactory();
        CloseMenu();
    }
}
