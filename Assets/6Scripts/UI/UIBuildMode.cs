using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildMode : MonoBehaviour
{
    [SerializeField] GameObject contentPrebuildIcon;
    [SerializeField] GameObject contentbuildSlotUI;
    [SerializeField] List<BuildUI> allBuildBtns;
    [SerializeField] GameObject buildUIParent;
    [SerializeField] GameObject buildMenu;

    [SerializeField] List<BuildSlot> allBuildIcons;
    [SerializeField] List<BuildUI> allBuildUI = new List<BuildUI>();
    public static UIBuildMode instance;
    private void Awake()
    {
        instance = this;
        buildUIParent.SetActive(false);
        FillAllBuildBtns();
    }
    public void CloseAllWindows()
    {
        HideBuildModeMenu();
        HideBuildsCatalog();
    }
    public void addPreBuild(BuildUI buildUI)
    {
        allBuildUI.Add(buildUI);
    }
    public void CloseAllPreBuild()
    {
        for (int i = 0; i < allBuildUI.Count; i++)
        {
            allBuildUI[i].prebuildGO.SetActive(false);
            allBuildUI[i].ResetUI();
        }
        CellMenuManager.instance.CloseAllMenu();
    }
    void FillAllBuildBtns()
    {
        allBuildBtns.Clear();
        for (int i = 0; i < contentPrebuildIcon.transform.childCount; i++)
        {
            allBuildBtns.Add(contentPrebuildIcon.transform.GetChild(i).GetChild(1).GetComponent<BuildUI>());
        }
        allBuildIcons.Clear();
        for (int i = 0; i < contentbuildSlotUI.transform.childCount; i++)
        {
            allBuildIcons.Add(contentbuildSlotUI.transform.GetChild(i).GetComponent<BuildSlot>());
        }
    }
    public void ShowBuildModeMenu(BuildSlot buildSlot)
    {
        HideBuildModeMenu();
        for (int i = 0; i < allBuildBtns.Count; i++)
        {
            if (allBuildBtns[i].buildCell_SO.cellName == buildSlot.buildCell_So.cellName)
            {
                buildUIParent.SetActive(true);
                allBuildBtns[i].transform.parent.gameObject.SetActive(true);
            }
        }
        CollectMarkManager.instance.HideAllMark();
        GlobalData.instance.HideAllBtn();
        buildUIParent.SetActive(true);
        buildMenu.SetActive(false);
        SoundController.instance.btnSound.Play();
    }
    public void HideBuildModeMenu()
    {
        for (int i = 0; i < allBuildBtns.Count; i++)
        {
            allBuildBtns[i].transform.parent.gameObject.SetActive(false);
        }
        CollectMarkManager.instance.ShowAllMark();
        GlobalData.instance.ShowAllBtn();
        buildUIParent.SetActive(false);
    }

    public void ShowBuildsCatalog()
    {
        HideBuildModeMenu();
        buildMenu.SetActive(true);
        BuildCell_So[] allBuildCells = GlobalData.instance.allCells.buildingCells;
        // Сортуємо масив за зростанням вартості
        Array.Sort(allBuildCells, (a, b) => GlobalData.instance.CalcCostProgressionByCount(IslandBuilding.Instance.GetCellCountByName(a.cellName), a.startCost).CompareTo(GlobalData.instance.CalcCostProgressionByCount(IslandBuilding.Instance.GetCellCountByName(b.cellName), b.startCost)));

        for (int i = 0; i < allBuildCells.Length; i++)
        {
            allBuildIcons[i].SetData(
                allBuildCells[i],
                GlobalData.instance.CalcCostProgressionByCount(IslandBuilding.Instance.GetCellCountByName(allBuildCells[i].cellName), allBuildCells[i].startCost),
                allBuildCells[i].costIconCode,
                ValutaData.ValutaType.Money
            );
        }
    }

    public void HideBuildsCatalog()
    {
        buildMenu.SetActive(false);
    }
}
