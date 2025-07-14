using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class IslandBuildZone : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    bool isMax = false;
    Animation anim;
    long cost;
    private void OnMouseUp()
    {
        if(isMax)
        {
            IncorrectActionAnim.instance.ShowMassage("You have reached the maximum number of islands", transform.position);
            return;
        }
        if (true)
        {
            
        }
        if (!EventSystem.current.IsPointerOverGameObject(0) && CameraMovement.instance.drag == false && GameData.instance.DecreaseMoney(cost))
        {
            GameObject island = Instantiate(IslandBuilding.Instance.islandPrefab[Random.Range(0, IslandBuilding.Instance.islandPrefab.Length)], transform.position, Quaternion.identity);
            island.transform.SetParent(IslandBuilding.Instance.gameObject.transform);

            IslandData islandData = island.GetComponent<IslandData>();
            islandData.GenerateCellsOnIsland(4);
            anim = island.GetComponent<Animation>();
            anim.Play();
            IslandBuilding.Instance.AddIsland(islandData, true);
            CameraMovement.instance.SetBorders();
            CalcPathInWater.instance.FindFreeWay();
            island.GetComponent<AudioSource>().Play();
        }
    }

    public void SetData(string text, bool isMax, long cost)
    {
        this.text.text = text;
        this.isMax = isMax;
        this.cost = cost;
        if (GameData.instance.GetMoney() < cost)
        {
            this.text.color = Color.red;
        }
    }
}
