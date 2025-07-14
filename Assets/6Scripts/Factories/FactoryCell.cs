using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FactoryCell : Cell
{
    public BuildCell_So buildCell_So;
    public GameObject ModelPos;
    public GameObject lvlModel;
    public int currentLVL;

    public override void OnMouseUp()
    {
        if (IslandBuildMode.instance.buildMode)
        {
            return;
        }
        if (CameraMovement.instance.drag == false && !EventSystem.current.IsPointerOverGameObject(0))
        {
            CellMenuManager.instance.OpenFactoryMenu(this, buildCell_So, gameObject);
            PlayAnim();
        }
    }
    public void SetLVL(int lvl)
    {
        if (lvl - 1 < 0 || buildCell_So.upgrades.Length < lvl)
        {
            return;
        }
        if (lvlModel != null)
        {
            Destroy(lvlModel);
        }
        GameObject go = Instantiate(buildCell_So.upgrades[lvl - 1].lvlPrefab, ModelPos.transform);
        lvlModel = go;
        currentLVL = lvl;
    }
    public bool UpgradeFactory()
    {
        if (currentLVL + 1 > buildCell_So.upgrades.Length)
        {
            return false;
        }
        currentLVL++;
        if (lvlModel != null)
        {
            Destroy(lvlModel);
        }
        GameObject go = Instantiate(buildCell_So.upgrades[currentLVL - 1].lvlPrefab, ModelPos.transform);
        lvlModel = go;
        IslandBuilding.Instance.SaveIlsland();
        return true;
    }
}
