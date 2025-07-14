using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IslandZoneController : MonoBehaviour, IBuildInteractionZone
{
    [SerializeField] FindIslandFactories islandChecker;
    public BuildZonesManager.hideType zoneHideType;
    public void ShowZone()
    {

        List<GameObject> cellOnIsland = islandChecker.FindCellOnIland();
        List<Vector3> connectionPos = new List<Vector3>();

        List<Zone> zones = new List<Zone>();
        for (int i = 0; i < cellOnIsland.Count; i++)
        {
            Factory factory = cellOnIsland[i].GetComponent<Factory>();
            string text = (factory.collectValutaCount <= 0 ? "+0" : "+" + factory.collectValutaCount) + ValutaData.instance.GetValutaDataByType(factory.valutaType).iconCode;

            zones.Add(new Zone(factory.gameObject.transform.position, text, BuildZonesManager.ZoneType.blue));
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
