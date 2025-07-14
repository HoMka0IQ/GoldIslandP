using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildZonesManager;

public class NeighborZoneController : MonoBehaviour, IBuildInteractionZone
{
    [SerializeField] FindNeighborCells neighborCells;
    public BuildZonesManager.hideType zoneHideType;
    private void Start()
    {
        if (neighborCells == null)
        {
            neighborCells.GetComponent<FindNeighborCells>();
        }
    }
    public void ShowZone()
    {
        ShowZone(1f);//1x is default value
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

        BuildZonesManager.instance.ShowZones(zones, zoneHideType);

    }

    private void OnDisable()
    {
        if (BuildZonesManager.instance != null)
        {
            BuildZonesManager.instance.HideAll();
        }
    }
}
