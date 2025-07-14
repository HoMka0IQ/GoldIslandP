using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IslandData : MonoBehaviour
{
    //must have the same ID as in the IslandBuilding.islandPrefab array
    public int islandTypeID;

    [SerializeField] Transform[] allPos = new Transform[16];
    [SerializeField] Cell_SO[] cellOnIsland = new Cell_SO[16];
    [SerializeField] GameObject[] allCellsGO = new GameObject[16];
    [SerializeField] Cell[] allCells = new Cell[16];
    [Range(0,100)]
    [SerializeField] float emptyCellChance;

    [SerializeField] float delayForShowinCells = 4.5f;
    public int islandPosInArray { get; private set; }

    public void SetPosInArray(int id)
    {
        islandPosInArray = id;
    }
    public Cell_SO[] GetCellsSO()
    {
        return cellOnIsland; 
    }
    public Cell[] GetCells()
    {
        return allCells;
    }
    public GameObject[] GetGOCells()
    {
        return allCellsGO;
    }
    public void RefreshBuildsDataExcept(int id)
    {
        for (int i = 0; i < cellOnIsland.Length; i++)
        {
            if (cellOnIsland[i].cellTypes == Cell_SO.CellType.Factory && i != id)
            {
                IDataRefreshable dataRefreshable;
                if (allCellsGO[i].TryGetComponent<IDataRefreshable>(out dataRefreshable))
                {
                    dataRefreshable.RefreshData();
                }
            }
        }
    }
    public void CellChanger(Cell_SO cell, int id)
    {
        cellOnIsland[id] = cell;
        Destroy(allCellsGO[id]);
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        if (cell.randomRotationOnSpawn)
        {
            rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
        allCellsGO[id] = InstantiateCell(cellOnIsland[id].cellPrefab, allPos[id], rot, id, cell);
        allCells[id].PlayAnim();
        IslandBuilding.Instance.SaveIlsland();
    }
    public void FillCellOnIsland(List<Cell_SO> cell_so, float dalayFroShowingCell)
    {
        for (int i = 0; i < cell_so.Count; i++)
        {
            cellOnIsland[i] = cell_so[i];

            Quaternion rot = Quaternion.Euler(0, 0, 0);
            if (cell_so[i].randomRotationOnSpawn)
            {
                rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }

            allCellsGO[i] = InstantiateCell(cell_so[i].cellPrefab, allPos[i], rot, i, cell_so[i]);

            allCells[i] = allCellsGO[i].GetComponent<Cell>();
            allCells[i].HideCell();
        }
        StartCoroutine(ShowingAnim(dalayFroShowingCell));
    }
    public void GenerateCellsOnIsland(float dalayFroShowingCell)
    {
        int numberOfEmptyCells = Mathf.FloorToInt(allPos.Length * (emptyCellChance / 100));

        List<int> emptyIndices = Enumerable.Range(0, allPos.Length).OrderBy(x => Random.value).Take(numberOfEmptyCells).ToList();
        int presentCount = GlobalData.instance.allCells.presentCells.Length;
        for (int i = 0; i < allPos.Length; i++)
        {
            Cell_SO randCell;
            if (emptyIndices.Contains(i))
            {
                if (presentCount > 0)
                {
                    randCell = GlobalData.instance.allCells.presentCells[Random.Range(0, GlobalData.instance.allCells.presentCells.Length)];
                    presentCount--;
                }
                else
                {
                    randCell = GlobalData.instance.allCells.emptyCells[Random.Range(0, GlobalData.instance.allCells.emptyCells.Length)];
                }

            }
            else
            {
                float chance = Random.Range(0f, 100f);
                if (chance <= GlobalData.instance.allCells.rareChance)
                {
                    randCell = GlobalData.instance.allCells.allRareCells[Random.Range(0, GlobalData.instance.allCells.allRareCells.Length)];
                }
                else
                {
                    randCell = GlobalData.instance.allCells.allCommonCells[Random.Range(0, GlobalData.instance.allCells.allCommonCells.Length)];
                }

            }

            cellOnIsland[i] = randCell;

            Quaternion rot = Quaternion.Euler(0, 0, 0);
            if (cellOnIsland[i].randomRotationOnSpawn)
            {
                rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }

            allCellsGO[i] = InstantiateCell(randCell.cellPrefab, allPos[i], rot, i, randCell);
        }
        StartCoroutine(ShowingAnim(dalayFroShowingCell));
    }
    IEnumerator ShowingAnim(float dalay)
    {


        yield return new WaitForSeconds(dalay);

        for (int i = 0; i < allCellsGO.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            allCells[i].ShowCell();
        }
    }
    public GameObject InstantiateCell(GameObject cellPrefab, Transform spawnTransform, Quaternion rot, int id, Cell_SO Cell_SO)
    {
        GameObject cell = Instantiate(cellPrefab, spawnTransform.position, Quaternion.identity);
        cell.transform.SetParent(spawnTransform, true);
        cell.transform.rotation = rot;
        Cell cellScript = cell.GetComponent<Cell>();
        allCells[id] = cellScript;
        cellScript.SetData(this, id, Cell_SO);
        cellScript.HideCell();
        return cell;
    }
}
