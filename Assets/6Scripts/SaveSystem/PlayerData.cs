using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public long money;
    public long flask;
    public long maxMoneyAmount;
    public List<SerializableUpgrade> upgrades = new List<SerializableUpgrade>();

    public PlayerData(GameData gameData) 
    { 
        money = gameData.GetMoney();
        flask = gameData.GetFlask();
        maxMoneyAmount = gameData.maxMoneyAmount;
        UpgradesLogic ul = UpgradesLogic.Instance;
        for (int i = 0; i < ul.upgrades.Length; i++)
        {
            upgrades.Add(new SerializableUpgrade(ul.upgrades[i].currentLVL, ul.upgrades[i].buildCell_SO.cellName));
        }

    }

}
[Serializable]
public class SerializableUpgrade
{
    public int currentLVL;
    public string buildCell_SO_Name;

    public SerializableUpgrade(int currentLVL, string buildCell_SO_Name)
    {
        this.currentLVL = currentLVL;
        this.buildCell_SO_Name = buildCell_SO_Name;
    }
}

