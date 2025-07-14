using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System;

public class IslandBuildMode : MonoBehaviour
{
    [SerializeField] CellMenu cellMenu;

    [SerializeField] Transform cameraPos;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject exitBuildModeBtn;

    public bool buildMode = false;

    public static IslandBuildMode instance;

    private void Awake()
    {
        instance = this;
    }


    public void ShowBuildZones(string layerName)
    {
        List<IslandData> islands = IslandBuilding.Instance.GetIslands();
        for (int i = 0; i < islands.Count; i++)
        {
            for (int j = 0; j < islands[i].GetCellsSO().Length; j++)
            {
                if (islands[i].GetGOCells()[j].layer == LayerMask.NameToLayer(layerName))
                {
                    islands[i].GetGOCells()[j].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }

    }
    public void HideBuildZones(string layerName)
    {
        List<IslandData> islands = IslandBuilding.Instance.GetIslands();
        for (int i = 0; i < islands.Count; i++)
        {
            for (int j = 0; j < islands[i].GetCellsSO().Length; j++)
            {
                if (islands[i].GetGOCells()[j].layer == LayerMask.NameToLayer(layerName))
                {
                    islands[i].GetGOCells()[j].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

    }

    public void OnBuildMode()
    {
        IslandBuilding.Instance.ShowZones();

        mainCamera.transform.DORotate(new Vector3(90, 0, 0), 1f);
        cameraPos.transform.DORotate(new Vector3(0, 0, 0), 1f);
        buildMode = true;
        //mainCamera.DOOrthoSize(30, 1f);
        cameraPos.DOMoveY(50, 1f);

        exitBuildModeBtn.SetActive(true);
        CollectMarkManager.instance.HideAllMark();
        GlobalData.instance.HideAllBtn();
    }
    public void OffBuildMode()
    {
        IslandBuilding.Instance.buildZoneParent.SetActive(false);

        cameraPos.transform.DORotate(new Vector3(0, -45, 0), 1f);
        mainCamera.transform.DORotate(new Vector3(65, -45, 0), 1f);
        buildMode = false;
        exitBuildModeBtn.SetActive(false);

        //mainCamera.DOOrthoSize(11, 1f);
        cameraPos.DOMoveY(25, 1f);
        CollectMarkManager.instance.ShowAllMark();
        GlobalData.instance.ShowAllBtn();
    }
}
