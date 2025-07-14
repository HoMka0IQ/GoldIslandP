using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindIslandFactories : MonoBehaviour
{
    [SerializeField] LayerMask islandLayer;
    [SerializeField] GameObject zone;
    [HideInInspector]
    public bool zoneIsOn;
    public BuildCell_So.FactoryType factoryType;
    public List<GameObject> factoryOnIsland;
    [SerializeField] IslandData currentIsland;

    private void Start()
    {
        currentIsland = GetIsland().GetComponent<IslandData>();
        FindCellOnIland();
    }
    public List<GameObject> FindCellOnIland()
    {
        if (this.factoryOnIsland.Count > 0)
        {
            this.factoryOnIsland.Clear();
        }
        currentIsland = GetIsland().GetComponent<IslandData>();
        Cell_SO[] cellOnIsland = currentIsland.GetCellsSO();
        GameObject[] allCells = currentIsland.GetGOCells();
        for (int i = 0; i < cellOnIsland.Length; i++)
        {
            if (cellOnIsland[i].cellTypes == Cell_SO.CellType.Factory)
            {
                BuildCell_So buildCell = (BuildCell_So)cellOnIsland[i];
                if (buildCell.factoryTypes == factoryType)
                {
                    this.factoryOnIsland.Add(allCells[i]);
                }

            }
        }
        return this.factoryOnIsland;
    }

    public GameObject GetIsland()
    {
        GameObject island;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 5, Vector3.down * 7, out hit, 100, islandLayer))
        {
            island = hit.collider.gameObject;
            return island;
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position + Vector3.up * 5, transform.position + Vector3.down * 7, Color.red);
    }
}
