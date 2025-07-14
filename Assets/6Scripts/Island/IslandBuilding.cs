using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IslandBuilding : MonoBehaviour
{
    [SerializeField] List<IslandData> islands;
    [SerializeField] List<Vector3> emptyPos;
    [SerializeField] List<IslandBuildZone> buildZones;
    [SerializeField] Vector3[] checkWays;

    [SerializeField] IslandBuildZone buildZonePrefab;
    public GameObject buildZoneParent;

    public GameObject[] islandPrefab;

    public LayerMask layerMask;

    public static IslandBuilding Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        buildZoneParent.SetActive(false);
        CheckBuildZone();
        for (int i = 0; i < islands.Count; i++)
        {
            islands[i].SetPosInArray(i);
        }
        
        LoadIslands();
    }
    public void ShowZones()
    {
        CheckBuildZone();
        buildZoneParent.SetActive(true);
    }
    public int GetCellCountByName(string cellName)
    {
        int count = 0;
        for (int i = 0; i < islands.Count; i++)
        {
            Cell_SO[] cells = islands[i].GetCellsSO();
            for (int j = 0; j < cells.Length; j++)
            {
                if (cells[j].cellName == cellName)
                {
                    count++;
                }
            }
        }
        return count;
    }
    public void SaveIlsland()
    {
        SaveSystem.SaveIsland(islands);
    }

    public void LoadIslands()
    {
        List<IslandSavesData> allIslands = SaveSystem.LoadIsland();

        if (allIslands == null)
        {
            GlobalData.instance.FirstPuck();
            return;
        }

        for (int i = 0; i < allIslands.Count; i++)
        {


            Vector3 pos = new Vector3(allIslands[i].position[0], allIslands[i].position[1], allIslands[i].position[2]);
            GameObject island = Instantiate(islandPrefab[allIslands[i].typeID], pos, Quaternion.identity);
            island.transform.SetParent(transform);

            IslandData islandData = island.GetComponent<IslandData>();
            AddIsland(islandData, false);

            List<Cell_SO> cellOnIsland = new List<Cell_SO>();
            for (int j = 0; j < allIslands[i].cellsName.Length; j++)
            {
                cellOnIsland.Add(GlobalData.instance.GetCellbyName(allIslands[i].cellsName[j]));
            }
            islandData.FillCellOnIsland(cellOnIsland, 0);

            for (int j = 0; j < allIslands[i].factoriesID.Count; j++)
            {
                FactoryCell factoryCell = islandData.GetCells()[allIslands[i].factoriesID[j]] as FactoryCell;
                factoryCell.SetLVL(allIslands[i].factoriesLVL[j]);
            }

            for (int j = 0; j < allIslands[i].naturalCellRemoveProcessStart.Length; j++)
            {
                NaturalCell naturalCell = islandData.GetCells()[j] as NaturalCell;
                if (naturalCell != null && allIslands[i].naturalCellRemoveProcessStart[j] == true)
                {
                    naturalCell.LoadRemoveData(allIslands[i].naturalCellRemoveTimer[j]);
                }
                
            }

        }
        CameraMovement.instance.SetBorders();
        if (islands.Count == 0)
        {
            GlobalData.instance.FirstPuck();
        }
        CalcPathInWater.instance.FindFreeWay();
    }


    public void firstIsland()
    {
        IslandData islandData = Instantiate(islandPrefab[0], Vector3.zero, Quaternion.identity).GetComponent<IslandData>();
        islandData.GenerateCellsOnIsland(0);
        AddIsland(islandData, true);
    }
    public List<IslandData> GetIslands()
    {
        return islands;
    }
    public List<Vector3> GetEmptyPos()
    {
        return emptyPos;
    }
    public void AddIsland(IslandData island, bool save)
    {
        islands.Add(island);
        island.SetPosInArray(islands.Count - 1);
        CheckBuildZone();

        if (save)
        {
            Invoke("SaveIlsland", 0.1f);
        }
    }
    public void CheckBuildZone()
    {
        if (buildZones.Count > 0)
        {
            foreach (IslandBuildZone obj in buildZones)
            {
                if (obj != null)
                {
                    Destroy(obj.gameObject);
                }
            }
            buildZones.Clear();
        }
        if (emptyPos.Count > 0)
        {
            emptyPos.Clear();
        }
        foreach (IslandData island in islands)
        {
            CheckSide(island.transform, island.transform.right * 10);
            CheckSide(island.transform, -island.transform.right * 10);
            CheckSide(island.transform, island.transform.forward * 10);
            CheckSide(island.transform, -island.transform.forward * 10);
        }

        List<Vector3> uniquePositions = emptyPos.Distinct().ToList();
        emptyPos = uniquePositions;

        InstantiateBuildZone();
    }
    public void InstantiateBuildZone()
    {
        for (int i = 0; i < emptyPos.Count; i++)
        {
            IslandBuildZone buildZone = Instantiate(buildZonePrefab, emptyPos[i], Quaternion.identity) as IslandBuildZone;
            if (islands.Count >= 50)
            {
                buildZone.SetData("MAX",true, GlobalData.instance.calcCostProgression(islands.Count, 7500));
            }
            else
            {
                buildZone.SetData(GlobalData.instance.ValueFormater(GlobalData.instance.calcCostProgression(islands.Count, 7500)) + "<sprite=0>", false, GlobalData.instance.calcCostProgression(islands.Count, 7500));
            }

            buildZone.transform.SetParent(buildZoneParent.transform);
            buildZones.Add(buildZone);
        }
    }
    public void CheckSide(Transform startPoint, Vector3 ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPoint.position, ray, out hit, ray.magnitude, layerMask))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Island") )
            {
                return;
            }
        }
        emptyPos.Add(startPoint.position + ray);
    }
    private void OnDrawGizmos()
    {
        if (islands.Count == 0)
        {
            return;
        }

        foreach (IslandData island in islands)
        {
            DrawRayWithHitCheck(island.transform, island.transform.right * 10);
            DrawRayWithHitCheck(island.transform, -island.transform.right * 10);
            DrawRayWithHitCheck(island.transform, island.transform.forward * 10);
            DrawRayWithHitCheck(island.transform, -island.transform.forward * 10);
        }
    }

    private void DrawRayWithHitCheck(Transform startPoint, Vector3 direction)
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(startPoint.position, direction, out hit, direction.magnitude, layerMask);
        Color rayColor = isHit ? Color.red : Color.green;

        Debug.DrawRay(startPoint.position, direction, rayColor, 1.0f);
    }
    private void OnApplicationQuit()
    {
        SaveIlsland();
    }
}
