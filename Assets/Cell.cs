using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{

    public Cell_SO cell_SO;
    [HideInInspector]
    public IslandData islandData;
    [HideInInspector]
    public int posInArray;

    public GameObject modelGO;



    [SerializeField] Animation showingAnim;

    public void HideCell()
    {
        modelGO.transform.localScale = Vector3.zero;
    }
    public void ShowCell()
    {
        if (showingAnim == null)
        {
            TryGetComponent<Animation>(out showingAnim);
        }
        showingAnim.Play("CellShowAnim");
    }
    public void SetData(IslandData islandData, int posInArray, Cell_SO cell_SO)
    {
        this.islandData = islandData;
        this.posInArray = posInArray;
        this.cell_SO = cell_SO;
    }

    public void PlayAnim()
    {
        if (showingAnim == null)
        {
            return;
        }
        SoundController.instance.cellSound.Play();
        showingAnim.Play("CellShowAnim");
    }

    public virtual void OnMouseUp()
    {

    }
}
