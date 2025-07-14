using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using static BuildZonesManager;
public class BuildUI : MonoBehaviour, IDragHandler, IEndDragHandler,IBeginDragHandler
{

    [Header("Ray")]
    Camera mainCamera;
    [SerializeField] LayerMask layer;

    [Header("Build")]
    public BuildCell_So buildCell_SO;
    [HideInInspector]
    public GameObject prebuildGO;
    IBuildInteractionZone buildInteractions;
    PreBuild preBuild;
    Vector3 lastPos;
    [SerializeField] Transform prebuildParent;
    public BuildZonesManager.hideType zoneHideType;
    [Header("Main")]
    Image image;
    bool dragging;
    void Start()
    {
        mainCamera = Camera.main;
        prebuildGO = Instantiate(buildCell_SO.prebuildPrefab, prebuildParent);
        UIBuildMode.instance.addPreBuild(this);
        prebuildGO.TryGetComponent<IBuildInteractionZone>(out buildInteractions);
        preBuild = prebuildGO.GetComponent<PreBuild>();
        preBuild.buildUI = this;
        prebuildGO.SetActive(false);
 
        image.sprite = buildCell_SO.icon;
    }
    private void OnEnable()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        transform.localPosition = Vector3.zero;
        Color color = image.color;
        color.a = 1;
        image.color = color;
    }

    void Update()
    {
        if (!dragging && prebuildGO.activeSelf == true)
        {
            transform.position = mainCamera.WorldToScreenPoint(prebuildGO.transform.position);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        UIBuildMode.instance.CloseAllPreBuild();
        IslandBuildMode.instance.ShowBuildZones(LayerMask.LayerToName(Mathf.RoundToInt(Mathf.Log(layer.value, 2))));
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        preBuild.HideActionBtns();
        transform.position = eventData.position;
        Ray ray = mainCamera.ScreenPointToRay(transform.position);
        RaycastHit hit;
        Color color = image.color;
        color.a = 0;
        image.color = color;

        if (Physics.Raycast(ray, out hit, 55f, layer))
        {
            Debug.DrawRay(ray.origin, ray.direction * 55f, Color.cyan);

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayerMask.LayerToName(Mathf.RoundToInt(Mathf.Log(layer.value, 2)))))
            {
                if (lastPos != hit.collider.gameObject.transform.position || prebuildGO.activeSelf == false)
                {
                    preBuild.goCellUnder = hit.collider.gameObject.GetComponent<Cell>();
                    prebuildGO.SetActive(true);
                    prebuildGO.transform.position = hit.collider.gameObject.transform.position;
                    lastPos = hit.collider.gameObject.transform.position;
                    SoundController.instance.cellSound.Play();
                    preBuild.ActionOnSet();
                    if (buildInteractions != null)
                    {
                        BuildZonesManager.instance.HideAll();
                        buildInteractions.ShowZone();
                    }
                }
                return;
            }

        }
        prebuildGO.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        preBuild.ShowActionBtns();
        if (prebuildGO.activeSelf == false)
        {
            ResetUI();
        }
        
    }
    public void ResetUI()
    {
        transform.localPosition = Vector3.zero;
        Color color = image.color;
        color.a = 1;
        image.color = color;
        IslandBuildMode.instance.HideBuildZones(LayerMask.LayerToName(Mathf.RoundToInt(Mathf.Log(layer.value, 2))));
    }


}
