using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Factory : MonoBehaviour
{
    int valueInside;
    [SerializeField] CollectorUI collectorUI;
    Animation moveAnim;
    protected BuildCell_So buildCell;

    [SerializeField] Animation factoryAnimation;
    [SerializeField] Sprite collectIcon;
    public ValutaData.ValutaType valutaType;

    public int collectValutaCount;

    protected FactoryCell FactoryCell;
    [SerializeField] protected float[] bonusByLocalLVL = {1,1.5f,2f};

    [SerializeField] int currentValutaInFactory_Info;
    [SerializeField] long collectFromFactory_Info;
    [SerializeField] long collectFromCells_Info;

    [SerializeField] protected BuildZonesManager.hideType hideType;

    protected virtual void Start()
    {
        buildCell = GetComponent<Cell>().cell_SO as BuildCell_So;
        FactoryCell = GetComponent<FactoryCell>();
        StartCoroutine(colectCoins());
        
        moveAnim = GetComponent<Animation>();   
        collectorUI = CollectMarkManager.instance.GetCollector(gameObject, valutaType);
        collectorUI.OnCollectValue += CollectValue;
    }
    public int GetMoneyInside()
    {
        return valueInside;
    }
    public void CearMoneyInside()
    {
        valueInside = 0;
        collectorUI.SetActive(false);
    }
    public int TakeMoney(int count)
    {
        int returnValue = 0;
        if (valueInside <= count)
        {
            returnValue = valueInside;
            valueInside = 0;
            if (collectorUI != null)
            {
                collectorUI.SetActive(false);
            }
            
        }
        if (valueInside > count)
        {
            returnValue = count;
            valueInside = valueInside - count;
        }

        return returnValue;

    }
    IEnumerator colectCoins()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            int calcCollectValue = (int)((GlobalData.instance.CalcCostProgressionByLVL(buildCell, buildCell.collectValue) + CalcCollectBonusValue()) * bonusByLocalLVL[FactoryCell.currentLVL]);

            collectFromFactory_Info = (long)(GlobalData.instance.CalcCostProgressionByLVL(buildCell, buildCell.collectValue) * bonusByLocalLVL[FactoryCell.currentLVL]);
            collectFromCells_Info = (long)(CalcCollectBonusValue() * bonusByLocalLVL[FactoryCell.currentLVL]);
            collectValutaCount = calcCollectValue;

            calcCollectValue = Mathf.Clamp(calcCollectValue, 0, int.MaxValue);

            valueInside += calcCollectValue;
            currentValutaInFactory_Info = valueInside;
            if (calcCollectValue <= 0 && factoryAnimation != null)
            {
                factoryAnimation.Stop();
            }
            else if(calcCollectValue > 0 && factoryAnimation != null)
            {
                factoryAnimation.Play();
            }
            if (valueInside > calcCollectValue * 6)
            {
                collectorUI.SetActive(true);
               
            }
            if (collectorUI.gameObject.activeSelf == true)
            {
                collectorUI.UpdateText(GlobalData.instance.ValueFormater(valueInside));
            }
            yield return new WaitForSeconds(1f);
        }
    }
    protected virtual int CalcCollectBonusValue()
    {
        return 0;
    }
    public void CollectValue()
    {
        CoinCollectionAnimManager.instance.InitCollectCoinAnim(transform.position, valutaType);
        GameData.instance.IncreaseByType(valutaType, valueInside);
        CearMoneyInside();
    }
    protected virtual void OnMouseUp()
    {
        if (IslandBuildMode.instance.buildMode)
        {
            return;
        }
        if (CameraMovement.instance.drag == false && !EventSystem.current.IsPointerOverGameObject(0))
        {
            moveAnim.Play();
            if (collectValutaCount > 0)
            {
                BuildZonesManager.instance.ShowTextOnly(transform.position, "+" + collectValutaCount + ValutaData.instance.GetValutaDataByType(valutaType).iconCode, BuildZonesManager.ZoneType.blue, hideType);
            }
            else
            {
                BuildZonesManager.instance.ShowTextOnly(transform.position, "0" + ValutaData.instance.GetValutaDataByType(valutaType).iconCode, BuildZonesManager.ZoneType.red, hideType);
            }

           
        }
    }
    private void OnDestroy()
    {
        if (collectorUI != null)
            Destroy(collectorUI.gameObject);
    }

}
