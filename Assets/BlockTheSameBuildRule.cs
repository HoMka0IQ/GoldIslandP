using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTheSameBuildRule : MonoBehaviour, BuildRules
{
    [SerializeField] Cell_SO cell_SO;
    [SerializeField] string ruleDescription;
    public bool AcceptRule(Cell_SO cell_SO)
    {
        IslandData id = GetComponent<PreBuild>().goCellUnder.islandData;
        for (int i = 0; i < id.GetCellsSO().Length; i++)
        {
            if (id.GetCellsSO()[i].cellName == this.cell_SO.cellName)
            {
                return false;
            }
        }
        return true;
    }
    
    public string RuleDescription()
    {
        return ruleDescription;
    }
}
