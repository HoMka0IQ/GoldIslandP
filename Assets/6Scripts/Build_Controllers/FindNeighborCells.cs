using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
public class FindNeighborCells : MonoBehaviour
{
    [Header("Check Zone")]
    public LayerMask checkLayer;

    Vector3[] allCheckPos = new Vector3[]
    {
        new Vector3(2.5f,0,0),
        new Vector3(-2.5f,0,0),
        new Vector3(2.5f,0,2.5f),
        new Vector3(2.5f,0,-2.5f),
        new Vector3(-2.5f,0,2.5f),
        new Vector3(-2.5f,0,-2.5f),
        new Vector3(0,0,-2.5f),
        new Vector3(0,0,2.5f)
    };
    public Collider[] colliders;
    [Space(15f)]
    [Header("Buff")]
    [SerializeField] Cell_SO.CellType checkBuffCellType;
    public int buffCellCount;
    public List<Vector3> allBuffPos;

    [Header("Debuff")]
    [SerializeField] Cell_SO.CellType checkDebuffCellType;
    public int debuffCellCount;
    public List<Vector3> allDebuffPos;

    [SerializeField] bool debuffYourselfType;
    [SerializeField] bool debuffGround;
    [Space(15f)]
    [Header("Other")]
    [SerializeField] Animation anim;
    Camera cam;
    bool zonesIsOn;
    public BuildCell_So buildCell;
  


    private void Start()
    {
        cam = Camera.main;
    }
    public Collider[] GetColliders()
    {
        return colliders;
    }
    List<Vector3> FindCellType(List<Cell> cells, Collider[] colliders, Cell_SO.CellType findType)
    {
        List<Vector3> pos = new List<Vector3>();
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].cell_SO.cellTypes == findType)
            {
                pos.Add(cells[i].transform.position);
            }
            
        }
        if (findType == Cell_SO.CellType.Empty)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null)
                {
                    pos.Add(allCheckPos[i] + transform.position);
                }
            }
        }
        return pos;
    }
    List<Vector3> FindSameType(List<Cell> cells)
    {
        List<Vector3> pos = new List<Vector3>();
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].cell_SO.cellName == buildCell.cellName)
            {
                pos.Add(cells[i].transform.position);
            }

        }
        return pos;
    }

    Collider[] FindAllColliders()
    {
        Collider[] colliders = new Collider[0];
        for (int i = 0; i < allCheckPos.Length; i++)
        {
            Vector3 position = allCheckPos[i] + transform.position + Vector3.up * (Vector3.one.y / 2);
            Collider[] cellColliders = Physics.OverlapBox(position, Vector3.one / 2, Quaternion.identity, checkLayer);
            if (cellColliders.Length > 0)
            {
                colliders = colliders.Concat(cellColliders).ToArray();
            }
            else
            {
                colliders = colliders.Concat(new Collider[1]).ToArray();
            }
        }
        return colliders;
    }
    List<Cell> CellsFromColliders(Collider[] colliders)
    {
        List<Cell> Cells = new List<Cell>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Cell cell;
            if (colliders[i] != null)
            {
                colliders[i].gameObject.TryGetComponent<Cell>(out cell);
                if (cell != null)
                {
                    Cells.Add(cell);
                }
            }
        }
        return Cells;
    }

    public void CalcNeighborCell()
    {
        colliders = FindAllColliders();

        List<Cell> Cells = CellsFromColliders(colliders);

        allBuffPos = FindCellType(Cells, colliders, checkBuffCellType);
        buffCellCount = allBuffPos.Count;
       
        allDebuffPos = FindCellType(Cells, colliders, checkDebuffCellType);
        if (debuffYourselfType)
            allDebuffPos.AddRange(FindSameType(Cells));
        debuffCellCount = allDebuffPos.Count;

    }
 
    public void MoveAnimPlay()
    {
        anim.Play();
    }
    void OnDrawGizmos()
    {
        for (int i = 0; i < allCheckPos.Length; i++)
        {
            Gizmos.color = Color.red;
            Vector3 position = allCheckPos[i] + transform.position + Vector3.up * (Vector3.one.y / 2);
            Gizmos.DrawCube(position, Vector3.one);
        }

    }

}
