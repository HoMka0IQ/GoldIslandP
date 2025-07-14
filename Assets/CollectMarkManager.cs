using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMarkManager : MonoBehaviour
{
    public GameObject collectMoneyUIPrefab;
    public GameObject collectFlaskUIPrefab;
    public GameObject parent;

    public List<GameObject> allCollectors = new List<GameObject>();

    private Camera mainCamera;
    public float checkInterval = 0.5f; // Як часто перевіряти об'єкти (оптимізація)

    public static CollectMarkManager instance;
    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        StartCoroutine(CheckCollectorsVisibility());

    }
    public void HideAllMark()
    {
        parent.SetActive(false);
    }
    public void ShowAllMark()
    {
        parent.SetActive(true);
    }
    public CollectorUI GetCollector(GameObject GO, ValutaData.ValutaType valutaType)
    {
        GameObject collectUI;
        if (valutaType == ValutaData.ValutaType.Flask)
        {
            collectUI = Instantiate(collectFlaskUIPrefab, GO.transform.position, Quaternion.identity);
        }
        else
        {
            collectUI = Instantiate(collectMoneyUIPrefab, GO.transform.position, Quaternion.identity);
        }
        allCollectors.Add(collectUI);
        CollectorUI cUI = collectUI.GetComponent<CollectorUI>().SetTarget(GO);
        collectUI.transform.SetParent(parent.transform);
        cUI.SetActive(false);
        return cUI;
    }

    // Перевірка видимості кожні 0.5 секунди
    private IEnumerator CheckCollectorsVisibility()
    {
        while (true)
        {
            foreach (var collector in allCollectors)
            {
                if (collector != null)
                {
                    collector.SetActive(IsInCameraView(collector.transform.position));
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    // Перевірка, чи знаходиться точка в полі зору камери
    private bool IsInCameraView(Vector3 worldPosition)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(worldPosition);
        return viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1 &&
               viewportPoint.z > 0;
    }
}
