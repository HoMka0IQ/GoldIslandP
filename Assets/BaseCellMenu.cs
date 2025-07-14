using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildZonesManager;

public abstract class BaseCellMenu<TCell, TCellSO> : MonoBehaviour
{
    [SerializeField] protected GameObject cellMenuUI;
    protected ConnectionScreenToWorld connectionScreenToWorld;

    [SerializeField] protected Animation menuAnim;

    protected bool openTimer;

    protected TCell currentCell;
    protected TCellSO currentCellSO;

    public hideType currentHideType;

    [SerializeField] protected CellData_SO cellData_SO;

    protected virtual void Awake()
    {
        connectionScreenToWorld = cellMenuUI.GetComponent<ConnectionScreenToWorld>();
    }

    protected virtual void Update()
    {
/*        if (Input.touchCount > 0 && !openTimer && !TouchChecker.instance.IsPointerOverUIObject(Input.GetTouch(0)))
        {
            CloseMenu();
        }*/
        if (!openTimer && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if ((currentHideType == hideType.HideOnTrueTouch && touch.phase == TouchPhase.Ended && TouchChecker.instance.GetTrueTouch()) ||
                (currentHideType == hideType.HideOnMove) ||
                (currentHideType == hideType.HideOnEndTouch && touch.phase == TouchPhase.Ended))
            {
                CloseMenu();
            }
        }
    }

    public virtual void CloseMenu()
    {
        cellMenuUI.SetActive(false);
    }

    public virtual void OpenMenu(TCell cell, TCellSO cellSO, GameObject go)
    {
        openTimer = true;
        cellMenuUI.SetActive(true);
        connectionScreenToWorld.setData(go);
        menuAnim.Play();
        currentCell = cell;
        currentCellSO = cellSO;
        Invoke(nameof(OffOpenTimer), 0.01f);
    }

    protected void OffOpenTimer()
    {
        openTimer = false;
    }

    // Абстрактні методи для реалізації в дочірніх класах
    public abstract void PerformAction();
}
