using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static BuildZonesManager;

public class FactoryWithNeighbor : Factory, IDataRefreshable
{
    [SerializeField] FindNeighborCells neighborCells;

    protected override void Start()
    {
        base.Start();

        neighborCells.CalcNeighborCell();
    }

    protected override int CalcCollectBonusValue()
    {
        return (neighborCells.buffCellCount * (int)GlobalData.instance.CalcCostProgressionByLVL(buildCell, buildCell.BonusCollectValue)) - (neighborCells.debuffCellCount * ((int)GlobalData.instance.CalcCostProgressionByLVL(buildCell, buildCell.BonusCollectValue) * 2));
    }
    public void ShowZone(float bonusByLocalLVL)
    {
        neighborCells.CalcNeighborCell();

        string buffText = "+" + GlobalData.instance.ValueFormater((long)(GlobalData.instance.CalcCostProgressionByLVL(neighborCells.buildCell, neighborCells.buildCell.BonusCollectValue) * bonusByLocalLVL)) + "" + neighborCells.buildCell.costIconCode;
        string debuffText = "-" + GlobalData.instance.ValueFormater((long)(GlobalData.instance.CalcCostProgressionByLVL(neighborCells.buildCell, neighborCells.buildCell.BonusCollectValue) * bonusByLocalLVL) * 2) + "" + neighborCells.buildCell.costIconCode;

        List<Zone> zones = new List<Zone>();

        for (int i = 0; i < neighborCells.allBuffPos.Count; i++)
        {
            zones.Add(new Zone(neighborCells.allBuffPos[i], buffText, BuildZonesManager.ZoneType.blue));
        }
        for (int i = 0; i < neighborCells.allDebuffPos.Count; i++)
        {
            zones.Add(new Zone(neighborCells.allDebuffPos[i], debuffText, BuildZonesManager.ZoneType.red));
        }

        

        if (collectValutaCount > 0)
        {
            BuildZonesManager.instance.ShowZonesWithText(zones, transform.position, "+" + collectValutaCount + ValutaData.instance.GetValutaDataByType(valutaType).iconCode, BuildZonesManager.ZoneType.blue, hideType);
        }
        else
        {
            BuildZonesManager.instance.ShowZonesWithText(zones, transform.position, "0" + ValutaData.instance.GetValutaDataByType(valutaType).iconCode, BuildZonesManager.ZoneType.red, hideType);
        }

    }
    protected override void OnMouseUp()
    {
        if (IslandBuildMode.instance.buildMode)
        {
            return;
        }
        base.OnMouseUp();
        if (CameraMovement.instance.drag == false && !EventSystem.current.IsPointerOverGameObject(0))
        {
            ShowZone(bonusByLocalLVL[FactoryCell.currentLVL]);
        }
    }
   
    public void RefreshData()
    {
        neighborCells.CalcNeighborCell();
    }
}
