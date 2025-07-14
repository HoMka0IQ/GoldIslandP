using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BankLogic : MonoBehaviour,IDataRefreshable
{
    [SerializeField] FindIslandFactories islandChecker;
    public List<FactoryForBank> factoryOnIsland = new List<FactoryForBank>();
    [SerializeField] BuildCell_So buildCell_So;


    [SerializeField] GameObject particlePrefab;
    [SerializeField] List<GameObject> particleObject;

    [SerializeField] int CollectCount_info;
    [SerializeField] int canCollect;

    private void Start()
    {
        RefreshData();
        StartCoroutine(CollectMoney());
        StartCoroutine(CollectAnim());
    }

    IEnumerator CollectMoney()
    {
        while (true)
        {
            canCollect = (int)GlobalData.instance.CalcCostProgressionByLVL(buildCell_So, buildCell_So.collectValue);
            CollectCount_info = 0;
            for (byte i = 0; i < factoryOnIsland.Count; i++)
            {
                if (factoryOnIsland[i] == null || factoryOnIsland[i].factory.valutaType != ValutaData.ValutaType.Money && canCollect > 0)
                {
                    RefreshData();
                    continue;
                }
                int valueTaken = factoryOnIsland[i].factory.TakeMoney(canCollect);
                factoryOnIsland[i].collentFromFactory = valueTaken;
                canCollect -= valueTaken;
                CollectCount_info += valueTaken;
                GameData.instance.IncreaseMoney(valueTaken);
                if (factoryOnIsland.Count > particleObject.Count)
                {
                    for (byte j = 0; j < factoryOnIsland.Count; j++)
                    {
                        GameObject go = Instantiate(particlePrefab, transform);
                        particleObject.Add(go);
                        go.SetActive(false);
                    }
                }
                

            }
            yield return new WaitForSeconds(1f);
            GameData.instance.AddMoneyPerSec(CollectCount_info, gameObject);
        }
    }
    IEnumerator CollectAnim()
    {
        while (true)
        {
            for (byte i = 0; i < factoryOnIsland.Count; i++)
            {
                yield return new WaitForSeconds(0.2f);
                if (factoryOnIsland[i] == null || factoryOnIsland[i].factory == null || particleObject[i] == null)
                {
                    RefreshData();
                    continue; // Пропускаємо знищений елемент
                }
                MoveObjectWithArc(particleObject[i], factoryOnIsland[i].factory.transform.position, transform.position, 1f, 2f);


            }
            yield return new WaitForSeconds(2f);
        }

    }
    public void MoveObjectWithArc(GameObject objectToMove, Vector3 startPoint, Vector3 endPoint, float duration, float arcHeight)
    {
        if (objectToMove == null || objectToMove.transform == null) return;

        objectToMove.SetActive(true);
        objectToMove.transform.position = startPoint;

        Vector3 middlePoint = Vector3.Lerp(startPoint, endPoint, 0.5f);
        middlePoint.y += arcHeight;

        Vector3[] path = new Vector3[] { startPoint, middlePoint, endPoint };

        objectToMove.transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (objectToMove != null && objectToMove.transform != null) // Перевірка перед зміною стану
                {
                    objectToMove.SetActive(false);
                }
            });
    }
    public void RefreshData()
    {
        Debug.Log("Bank refreshing", gameObject);
        if (factoryOnIsland.Count > 0)
        {
            factoryOnIsland.Clear();
        }
        List<GameObject> cellOnIsland = islandChecker.FindCellOnIland();
        for (int i = 0; i < cellOnIsland.Count; i++)
        {
            factoryOnIsland.Add(new FactoryForBank(cellOnIsland[i].GetComponent<Factory>(), 0));
        }
    }
}
public class FactoryForBank
{
    public Factory factory;
    public int collentFromFactory;

    public FactoryForBank(Factory factory, int collentFromFactory)
    {
        this.factory = factory;
        this.collentFromFactory = collentFromFactory;
    }
}
