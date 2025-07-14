using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CellMenuManager : MonoBehaviour
{
    [SerializeField] CellMenu cellMenu;
    [SerializeField] FactoryCellMenu factoryMenu;
    [SerializeField] PresentCellMenu presentMenu;
    public static CellMenuManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void OpenCellMenu(NaturalCell cell, Cell_SO cell_So, GameObject go)
    {
        cellMenu.OpenMenu(cell, cell_So, go);

    }
    public void CloseCellMenu()
    {
        cellMenu.CloseMenu();
    }
    public void OpenFactoryMenu(FactoryCell cell, BuildCell_So buildCell_So, GameObject go)
    {
        factoryMenu.OpenMenu(cell, buildCell_So, go);

    }
    public void CloseFactoryMenu()
    {
        factoryMenu.CloseMenu();
    }
    public void OpenPresentMenu(PresentCell cell, PresentCell_SO presentCell_SO, GameObject go)
    {
        presentMenu.OpenMenu(cell, presentCell_SO, go);

    }
    public void ClosePresentMenu()
    {
        presentMenu.CloseMenu();
    }
    public void CloseAllMenu()
    {

    }
}
