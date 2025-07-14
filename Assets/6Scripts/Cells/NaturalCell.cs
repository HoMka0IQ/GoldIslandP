using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NaturalCell : Cell
{
    public bool removeProcess = false;
    public float removeTimer = 900f;
    GameObject removeEffect;
    public void StartRemoveProcess()
    {
        removeEffect = Instantiate(GlobalData.instance.buildDestroyerPrefab, transform.position, Quaternion.identity);
        removeEffect.transform.SetParent(modelGO.transform);
        removeEffect.transform.localScale = Vector3.one;
        removeProcess = true;
    }
    public void LoadRemoveData(float timer)
    {
        Debug.Log("Here");
        StartRemoveProcess();
        removeTimer = timer;
    }
    private void Update()
    {
        if (removeProcess)
        {
            removeTimer -= Time.deltaTime;
            if (removeTimer <= 0)
            {
                int cellIdOnIsland = posInArray;
                int islandId = islandData.islandPosInArray;
                Cell_SO emptyCell = GlobalData.instance.allCells.emptyCells[Random.Range(0, GlobalData.instance.allCells.emptyCells.Length)];
                IslandBuilding.Instance.GetIslands()[islandId].CellChanger(emptyCell, cellIdOnIsland);
                Destroy(removeEffect);
            }
        }
    }
    public override void OnMouseUp()
    {

        if (IslandBuildMode.instance.buildMode || CameraMovement.instance.drag == true || EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }
        if (removeProcess)
        {
            BuildZonesManager.instance.ShowTimerText(transform.position, removeTimer, BuildZonesManager.ZoneType.blue, BuildZonesManager.hideType.HideOnEndTouch);
            SoundController.instance.cellSound.Play();
            PlayAnim();
            return;
        }
        CellMenuManager.instance.OpenCellMenu(this, cell_SO, gameObject);
        SoundController.instance.cellSound.Play();
        PlayAnim();

    }

}
