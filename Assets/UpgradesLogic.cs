using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradesLogic : MonoBehaviour
{
    public Upgrades[] upgrades;
    public BuildSlot[] updateSlots;

    public static UpgradesLogic Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Sort()
    {
        Array.Sort(upgrades, (a, b) => GlobalData.instance.CalcCostProgressionByLVL(a.buildCell_SO, a.buildCell_SO.updateCost).CompareTo(GlobalData.instance.CalcCostProgressionByLVL(b.buildCell_SO, b.buildCell_SO.updateCost)));
    }
    public void SetDescription()
    {
        Sort();
        for (int i = 0; i < upgrades.Length; i++)
        {
            Debug.Log("aa" + GlobalData.instance.CalcCostProgressionByLVL(upgrades[i].buildCell_SO, upgrades[i].buildCell_SO.updateCost));
            long cost = GlobalData.instance.CalcCostProgressionByLVL(upgrades[i].buildCell_SO, upgrades[i].buildCell_SO.updateCost);
            updateSlots[i].SetData(upgrades[i].buildCell_SO, cost, upgrades[i].buildCell_SO.updateIconCode, ValutaData.ValutaType.Flask);
        }
    }
    public int GetLvl(BuildCell_So buildCell_So)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].buildCell_SO.cellName == buildCell_So.cellName)
            {
                return upgrades[i].currentLVL;
            }
        }
        return -1;
    }
    public void UpdateBuild(BuildSlot buildSlot)
    {
        if (GameData.instance.DecreaseFlask((int)GlobalData.instance.CalcCostProgressionByLVL(buildSlot.buildCell_So, buildSlot.buildCell_So.updateCost)) == false)
        {
            return;
        }
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].buildCell_SO.cellName == buildSlot.buildCell_So.cellName)
            {
                upgrades[i].currentLVL++;
                GameData.instance.SaveData();
                
            }
        }
        SetDescription();
        SoundController.instance.btnSound.Play();
    }


}
