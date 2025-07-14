using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BankZoneController : MonoBehaviour, IBuildInteractionZone
{
    [SerializeField] FindIslandFactories islandChecker;
    [SerializeField] BankLogic bankLogic;
    public BuildZonesManager.hideType zoneHideType;
    public void ShowZone()
    {

        List<GameObject> cellOnIsland = islandChecker.FindCellOnIland();
        List<Vector3> connectionPos = new List<Vector3>();

        List<Zone> zones = new List<Zone>();

        List<FactoryForBank> factoryOnIsland = bankLogic.factoryOnIsland;
        for (int i = 0; i < factoryOnIsland.Count; i++)
        {
            if (factoryOnIsland[i] == null || factoryOnIsland[i].factory == null)
            {
                continue; // Пропускаємо знищений елемент
            }
            string text = (factoryOnIsland[i].collentFromFactory <= 0 ? "+0" : "+" + factoryOnIsland[i].collentFromFactory) + ValutaData.instance.GetValutaDataByType(factoryOnIsland[i].factory.valutaType).iconCode;

            zones.Add(new Zone(factoryOnIsland[i].factory.gameObject.transform.position, text, BuildZonesManager.ZoneType.blue));
        }
        BuildZonesManager.instance.ShowZones(zones, zoneHideType);
    }
    private void OnMouseUp()
    {
        if (CameraMovement.instance.drag == false && !EventSystem.current.IsPointerOverGameObject(0))
        {
            ShowZone();
        }
    }
    private void OnDisable()
    {
        if (BuildZonesManager.instance != null)
        {
            BuildZonesManager.instance.HideAll();
        }
    }
}