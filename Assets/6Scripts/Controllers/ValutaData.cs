using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValutaData : MonoBehaviour
{
    public string flaskIconCode;
    public Sprite flaskIcon;
    public string moneyIconCode;
    public Sprite moneyIcon;
    public static ValutaData instance;
    private void Awake()
    {
        instance = this;
    }
    public enum ValutaType
    {
        Money,
        Flask
    }
    public Valuta GetValutaDataByType(ValutaType valuta)
    {
        switch (valuta)
        {
            case ValutaType.Money:
                return new Valuta(GameData.instance.GetMoney(), moneyIconCode, moneyIcon);
            case ValutaType.Flask:
                return new Valuta(GameData.instance.GetFlask(), flaskIconCode, flaskIcon);
            default:
                throw new ArgumentException($"Unsupported valuta type: {valuta}");
        }
    }
}
public class Valuta
{
    public long count;
    public string iconCode;
    public Sprite icon;
    public Valuta(long count, string iconCode, Sprite icon)
    {
        this.count = count;
        this.iconCode = iconCode;
        this.icon = icon;
    }
}
