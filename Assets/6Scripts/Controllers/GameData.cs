using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] long money;
    [SerializeField] long flask;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text flaskText;


    public long maxMoneyAmount;
    [Header("Money per sec")]
    [SerializeField] GameObject parentMoneyPerSec;
    [SerializeField] TMP_Text textMoneyPerSec;
    [SerializeField] List<MoneyCollector> moneyCollector;

    public static GameData instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ReloadFlaskText();
        ReloadMoneyText();
        LoadData();
        StartCoroutine(SaveDataTimer());
    }
    public void SaveData()
    {
        SaveSystem.SavePlayerData(this);
    }
    IEnumerator SaveDataTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            SaveData();
        }
    }

    public void LoadData()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData();
        if (playerData == null)
        {
            return;
        }
        money = playerData.money;
        flask = playerData.flask;
        maxMoneyAmount = playerData.maxMoneyAmount;
        for (int i = 0; i < playerData.upgrades.Count; i++)
        {
            for (int t = 0; t < playerData.upgrades.Count; t++)
            {
                if (playerData.upgrades[i].buildCell_SO_Name == UpgradesLogic.Instance.upgrades[t].buildCell_SO.cellName)
                {
                    UpgradesLogic.Instance.upgrades[t].currentLVL = playerData.upgrades[i].currentLVL;
                    break;
                }
            }
        }
        ReloadFlaskText();
        ReloadMoneyText();
    }
    public void IncreaseByType(ValutaData.ValutaType valutaType, int count)
    {
        if (valutaType == ValutaData.ValutaType.Flask)
        {
            IncreaseFlask(count);
        }
        if (valutaType == ValutaData.ValutaType.Money)
        {
            IncreaseMoney(count);
        }
    }
    #region Money
    //Money logic_________
    public void AddMoneyPerSec(long count, GameObject go)
    {
        for (int i = 0; i < moneyCollector.Count; i++)
        {
            if (go == moneyCollector[i].go)
            {
                moneyCollector[i].count = count;
                SetMoneyPerSec();
                return;
            }
        }
        moneyCollector.Add(new MoneyCollector(count, go));
        SetMoneyPerSec();
    }
    void SetMoneyPerSec()
    {
        long count = 0; 
        for (int i = 0; i < moneyCollector.Count; i++)
        {
            count += moneyCollector[i].count;
        }
        if (count > 0)
        {
            parentMoneyPerSec.SetActive(true);
        }
        textMoneyPerSec.text = "+" + GlobalData.instance.ValueFormater(count);
    }
    public long GetMoney()
    {
        return money;
    }
    public void IncreaseMoney(long count)
    {
        money += count;
        if (money > maxMoneyAmount)
        {
            maxMoneyAmount = money;
        }
        ReloadMoneyText();
    }
    public bool DecreaseMoney(long count)
    {
        if (money < count)
        {
            return false;
        }
        else
        {
            money -= count;
        }
        ReloadMoneyText();
        return true;
    }
    public void ReloadMoneyText()
    {
        moneyText.text = GlobalData.instance.ValueFormater(money);
    }
    #endregion
    #region Flask
    //Flask logic_________
    public long GetFlask()
    {
        return flask;
    }
    public void IncreaseFlask(int count)
    {
        flask += count;
        ReloadFlaskText();
    }
    public bool DecreaseFlask(int count)
    {
        if (flask < count)
        {
            return false;
        }
        else
        {
            flask -= count;
        }
        ReloadFlaskText();
        return true;
    }
    public void ReloadFlaskText()
    {
        flaskText.text = GlobalData.instance.ValueFormater(flask);
    }
    #endregion
    public void add1000()
    {
        IncreaseMoney(1000);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }


    
}
[Serializable]
public class MoneyCollector
{
    public long count;
    public GameObject go;

    public MoneyCollector(long count, GameObject go)
    {
        this.count = count;
        this.go = go;
    }
}
