using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellMenu : BaseCellMenu<NaturalCell, object>
{

    public override void PerformAction()
    {
        ConfirmationDialog.instance.SetDialog(Action, "Do you want to remove this cell?");
        CloseMenu();
    }
    public void Action()
    {
        currentCell.StartRemoveProcess();
        currentCell.gameObject.layer = 0;
        CloseMenu();
    }
}
