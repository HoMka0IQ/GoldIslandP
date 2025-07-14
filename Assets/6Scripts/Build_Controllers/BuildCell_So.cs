using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Cells", menuName = "Cells/New Build Cell")]
public class BuildCell_So : Cell_SO
{
    public GameObject prebuildPrefab;
    public Sprite icon;
    public FactoryType factoryTypes;
    [Header("Vayuta")]
    public string costIconCode;
    public int startCost;
    public string updateIconCode;
    public int updateCost;

    [Header("Upgrade")]
    public UpgradeFactory[] upgrades;

    [Header("Production")]
    public int collectValue;
    public int BonusCollectValue;


    public enum FactoryType
    {
        None,
        Collector,
        MoneyFactory,
        FlaskFactory,
    }


    [Header("Other")]
    [TextArea(3, 10)]
    public string description;
}
[Serializable]
public class UpgradeFactory
{
    public int lvl;
    public long cost;
    public GameObject lvlPrefab;
}
