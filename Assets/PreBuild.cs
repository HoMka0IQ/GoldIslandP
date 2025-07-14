using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PreBuild : MonoBehaviour
{
    Camera cam;
    [Header("Main")]
    [SerializeField] Cell_SO cell_SO;
    public Cell goCellUnder;
    public BuildUI buildUI;
    [Header("UI")]
    [SerializeField] GameObject actionBtnsCenter;
    [SerializeField] Animation actionBtnsAnim;
    [SerializeField] Animation showingAnim;

    [SerializeField] FindNeighborCells findNeighborCells;

    void Start()
    {
        cam = Camera.main;

        if (actionBtnsAnim == null)
        {
            actionBtnsAnim = actionBtnsCenter.GetComponent<Animation>();
        }
    }
    public void ActionOnSet()
    {

        if (showingAnim == null)
        {
            showingAnim = GetComponent<Animation>();
        }
        showingAnim.Play();


        if (findNeighborCells == null)
        {
            if (TryGetComponent<FindNeighborCells>(out findNeighborCells) == false)
            {
                findNeighborCells = gameObject.AddComponent<FindNeighborCells>();
                findNeighborCells.checkLayer = LayerMask.GetMask("Cell");
            }
        }
        findNeighborCells.CalcNeighborCell();
        Debug.Log("Calc");
    }
    void Update()
    {
        if (actionBtnsCenter.activeSelf == true)
        {
            actionBtnsCenter.transform.position = cam.WorldToScreenPoint(transform.position);
        }
        
    }
    public void AcceptBtn()
    {
        BuildCell_So buildCell_So = cell_SO as BuildCell_So;
        BuildRules br;
        if(TryGetComponent<BuildRules>(out br) && br.AcceptRule(cell_SO) == false)
        {
            Debug.Log(br.RuleDescription());
            IncorrectActionAnim.instance.ShowMassage(br.RuleDescription(), transform.position);
            gameObject.SetActive(false);
            buildUI.ResetUI();
            UIBuildMode.instance.HideBuildModeMenu();
            
            return;
        }
        if (GameData.instance.DecreaseMoney(GlobalData.instance.CalcCostProgressionByCount(IslandBuilding.Instance.GetCellCountByName(buildCell_So.cellName),buildCell_So.startCost)) == false)
        {
            return;
        }

        SoundController.instance.buildAcceptSound.Play();
        Cell cell = goCellUnder;
        cell.islandData.CellChanger(cell_SO, cell.posInArray);
        cell.islandData.RefreshBuildsDataExcept(cell.posInArray);

        for (int i = 0; i < findNeighborCells.colliders.Length; i++)
        {
            IDataRefreshable dataRefreshable;
            if (findNeighborCells.colliders[i] != null && findNeighborCells.colliders[i].gameObject.TryGetComponent<IDataRefreshable>(out dataRefreshable))
            {
                dataRefreshable.RefreshData();
            }
        }

        gameObject.SetActive(false);
        buildUI.ResetUI();
        UIBuildMode.instance.HideBuildModeMenu();

    }
    public void CancelBtn()
    {
        gameObject.SetActive(false);
        buildUI.ResetUI();
        UIBuildMode.instance.HideBuildModeMenu();
        UIBuildMode.instance.CloseAllPreBuild();
    }
    public void ShowActionBtns()
    {
        actionBtnsCenter.SetActive(true);
        actionBtnsAnim.Play();
    }
    public void HideActionBtns()
    {
        actionBtnsCenter.SetActive(false);
    }
}
