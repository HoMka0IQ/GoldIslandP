using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PresentCell : Cell
{
    [SerializeField] PresentCell_SO PresentCell_SO;
    public override void OnMouseUp()
    {
        if (IslandBuildMode.instance.buildMode)
        {
            return;
        }
        if (CameraMovement.instance.drag == false && !EventSystem.current.IsPointerOverGameObject(0))
        {
            CellMenuManager.instance.OpenPresentMenu(this, PresentCell_SO, gameObject);
            PlayAnim();
        }
    }
}
