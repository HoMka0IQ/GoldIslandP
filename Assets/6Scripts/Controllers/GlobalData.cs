using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public CellData_SO allCells;
    public GameObject UICanvas;

    public RectTransform coinPos;
    public RectTransform flaskPos;

    public GameObject btnsParent;

    public GameObject buildDestroyerPrefab;

    public static GlobalData instance;
    private void Awake()
    {
        instance = this;
    }
    public void HideAllBtn()
    {
        btnsParent.SetActive(false);
    }
    public void ShowAllBtn()
    {
        btnsParent.SetActive(true);
    }
    public List<Cell_SO> GetCells()
    {
        List<Cell_SO> cells = new List<Cell_SO>();
        cells.AddRange(allCells.allCommonCells);
        cells.AddRange(allCells.allRareCells);
        cells.AddRange(allCells.allEpicCells);
        cells.AddRange(allCells.emptyCells);
        cells.AddRange(allCells.buildingCells);
        cells.AddRange(allCells.presentCells);
        return cells;
    }
    public Cell_SO GetCellbyName(string name)
    {
        List<Cell_SO> availableCells = GetCells();
        foreach (Cell_SO cell in availableCells)
        {
            if (cell.cellName == name)
            {
                return cell;
            }
        }
        Debug.LogError("Not found cell_so by name :" + name);
        return null;
    }
    public string ValueFormater(long amount)
    {
        if (amount >= 1_000_000_000_000_000)
        {
            double result = amount / 1_000_000_000_000_000.0;
            return FormatWithRounding(result) + "Q";
        }
        else if (amount >= 1_000_000_000_000)
        {
            double result = amount / 1_000_000_000_000.0;
            return FormatWithRounding(result) + "T";
        }
        else if (amount >= 1_000_000_000)
        {
            double result = amount / 1_000_000_000.0;
            return FormatWithRounding(result) + "B";
        }
        else if (amount >= 1_000_000)
        {
            double result = amount / 1_000_000.0;
            return FormatWithRounding(result) + "M";
        }
        else if (amount >= 1_000)
        {
            double result = amount / 1_000.0;
            return FormatWithRounding(result) + "k";
        }
        else
        {
            return amount.ToString();
        }
    }

    public void FirstPuck()
    {
        IslandBuilding.Instance.firstIsland();
        GameData.instance.IncreaseMoney(1500);
        GameData.instance.IncreaseFlask(500);

        GameData.instance.SaveData();
    }

    private string FormatWithRounding(double value)
    {
        if (value >= 100)
        {
            return Math.Round(value).ToString();
        }
        else
        {
            return value.ToString("0.#");
        }
    }
    public long calcCostProgression(int count, int startCost)
    {
        long endValue = startCost;
        for (int i = 0; i < count - 1; i++)
        {
            endValue = (long)(endValue * 2f);
        }
        return endValue;
    }
    public long CalcCostProgressionByCount(int count, int startCost)
    {
        float growthFactor = 2f + Mathf.Log(count + 1); // Більше значення count — більше зростання
        long endValue = startCost;

        for (int i = 0; i < count; i++)
        {
            endValue = (long)(endValue * growthFactor);
        }

        return endValue;
    }

    public long CalcCostProgressionByLVL(BuildCell_So buildCell_So, int startCost)
    {
        long endValue = startCost;
        int lvl = UpgradesLogic.Instance.GetLvl(buildCell_So);
        for (int i = 0; i < lvl - 1; i++)
        {
            endValue = endValue * 2;
        }

        return endValue;
    }
}
