using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildSlot : MonoBehaviour
{
    [HideInInspector]
    public BuildCell_So buildCell_So;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text lvlText;
    [SerializeField] TMP_Text buildName;
    [SerializeField] Image icon;

    [SerializeField] GameObject block;
    [SerializeField] TMP_Text blockText;

    UpgradesLogic upgradesLogic;

    [SerializeField] UnityEvent action;
    public void ActionBtn()
    {
        action.Invoke();
    }
    public void SetData(BuildCell_So build_SO, long cost, string iconCode, ValutaData.ValutaType valutaType)
    {
        buildName.text = build_SO.title.ToUpper();

        int currentLVL = UpgradesLogic.Instance.GetLvl(build_SO);

        costText.text = GlobalData.instance.ValueFormater(cost) + iconCode;


        List<object> argList = new List<object>();
        argList.Add(GlobalData.instance.ValueFormater((long)GlobalData.instance.CalcCostProgressionByLVL(build_SO, build_SO.collectValue)));
        argList.Add(GlobalData.instance.ValueFormater((long)GlobalData.instance.CalcCostProgressionByLVL(build_SO, build_SO.BonusCollectValue)));

        descriptionText.text = string.Format(build_SO.description, argList.ToArray());
        lvlText.text = "LVL." + currentLVL;
        icon.sprite = build_SO.icon;

        buildCell_So = build_SO;
        BlockChecker(cost, valutaType);

    }

    public void BlockChecker(long cost, ValutaData.ValutaType valutaType)
    {
        Valuta valuta = ValutaData.instance.GetValutaDataByType(valutaType);
        if (valuta.count < cost)
        {
            block.SetActive(true);
            blockText.text = GlobalData.instance.ValueFormater(valuta.count) + valuta.iconCode + "/" + GlobalData.instance.ValueFormater(cost) + valuta.iconCode;
        }
        else
        {
            block.SetActive(false);
        }
    }

}
