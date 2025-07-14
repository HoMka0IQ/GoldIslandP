using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildZonesManager : MonoBehaviour
{
    [SerializeField] GameObject[] zones;
    [SerializeField] ZoneCellData[] zonesCellData;

    [SerializeField] TextCellData[] zonesText;
    [SerializeField] TextCellData[] textOnly;

    [SerializeField] Color textBuffColor;
    [SerializeField] Color textDebuffColor;

    [SerializeField] Material zoneBuffMat;
    [SerializeField] Material zoneDebuffMat;

    [SerializeField] TextCellData timerCellData;
    [SerializeField] float timerTime;
    float _timer;

    hideType currentHideType;
    public enum hideType
    {
        HideOnMove,
        HideOnTrueTouch,
        HideOnEndTouch,
        NotHide
    }

    ZoneType currentType;
    public enum ZoneType
    {
        blue,
        red,
    }

    public static BuildZonesManager instance;

    enum ZoneState { Idle, Showing, Active }
    ZoneState _state;
    float _stateDelay;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (_state == ZoneState.Showing && Time.time >= _stateDelay)
        {
            _state = ZoneState.Active;
        }

        if (_state == ZoneState.Active && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if ((currentHideType == hideType.HideOnTrueTouch && touch.phase == TouchPhase.Ended && TouchChecker.instance.GetTrueTouch()) ||
                (currentHideType == hideType.HideOnMove) ||
                (currentHideType == hideType.HideOnEndTouch && touch.phase == TouchPhase.Ended))
            {
                HideAll();
            }
        }

        if (timerCellData != null && timerTime > 0)
        {
            timerTime -= Time.deltaTime;
            if (timerTime + 1 < _timer)
            {
                timerCellData.SetData(textBuffColor, FormatTime(timerTime));
                _timer = timerTime;
            }
        }
    }

    public void ShowZones(List<Zone> zoneData, hideType zoneHideType)
    {
        HideAll();
        currentHideType = zoneHideType;
        _state = ZoneState.Showing;
        _stateDelay = Time.time + 0.01f;

        foreach (Zone zone in zoneData)
        {
            SetZone(zone);
        }
    }

    public void ShowTextOnly(Vector3 pos, string text, ZoneType zoneType, hideType zoneHideType)
    {
        HideAll();
        currentHideType = zoneHideType;
        _state = ZoneState.Showing;
        _stateDelay = Time.time + 0.01f;

        SetTextOnly(pos, zoneType == ZoneType.blue ? textBuffColor : textDebuffColor, text);
    }

    public void ShowZonesWithText(List<Zone> zoneData, Vector3 textPos, string text, ZoneType zoneType, hideType zoneHideType)
    {
        HideAll();
        currentHideType = zoneHideType;
        _state = ZoneState.Showing;
        _stateDelay = Time.time + 0.01f;

        foreach (Zone zone in zoneData)
        {
            SetZone(zone);
        }

        SetTextOnly(textPos, zoneType == ZoneType.blue ? textBuffColor : textDebuffColor, text);
    }

    public void ShowTimerText(Vector3 pos, float currentTime, ZoneType zoneType, hideType zoneHideType)
    {
        HideAll();
        currentHideType = zoneHideType;
        _state = ZoneState.Showing;
        _stateDelay = Time.time + 0.01f;

        timerCellData = SetTextOnly(pos, zoneType == ZoneType.blue ? textBuffColor : textDebuffColor, FormatTime(currentTime));
        timerTime = currentTime;
        _timer = currentTime;
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void SetZone(Zone zone)
    {
        for (int i = 0; i < zones.Length; i++)
        {
            if (!zones[i].activeSelf)
            {
                zones[i].SetActive(true);
                zones[i].transform.position = zone.pos;
                zonesCellData[i].setData(zone.zoneType == ZoneType.blue ? zoneBuffMat : zoneDebuffMat);

                SetZoneText(zone.pos, zone.zoneType == ZoneType.blue ? textBuffColor : textDebuffColor, zone.text);
                break;
            }
        }
    }

    private void SetZoneText(Vector3 pos, Color textColor, string text)
    {
        for (int i = 0; i < zonesText.Length; i++)
        {
            if (!zonesText[i].gameObject.activeSelf)
            {
                zonesText[i].gameObject.SetActive(true);
                zonesText[i].SetData(textColor, text);
                zonesText[i].GetComponent<ConnectionScreenToWorld>().setData(pos);
                break;
            }
        }
    }

    private TextCellData SetTextOnly(Vector3 pos, Color textColor, string text)
    {
        for (int i = 0; i < textOnly.Length; i++)
        {
            if (!textOnly[i].gameObject.activeSelf)
            {
                textOnly[i].gameObject.SetActive(true);
                textOnly[i].SetData(textColor, text);
                textOnly[i].GetComponent<ConnectionScreenToWorld>().setData(pos);
                return textOnly[i];
            }
        }
        return null;
    }

    public void HideAll()
    {
        HideAllZones();
        HideAllTextOnly();
        _state = ZoneState.Idle;
    }

    void HideAllTextOnly()
    {
        foreach (var text in textOnly) text.gameObject.SetActive(false);
        timerCellData = null;
        timerTime = 0;
        _timer = 0;
    }

    void HideAllZones()
    {
        foreach (var zone in zones) zone.SetActive(false);
        foreach (var text in zonesText) text.gameObject.SetActive(false);
    }
}

public class Zone
{
    public Vector3 pos;
    public string text;
    public BuildZonesManager.ZoneType zoneType;

    public Zone(Vector3 pos, string text, BuildZonesManager.ZoneType zoneType)
    {
        this.pos = pos;
        this.text = text;
        this.zoneType = zoneType;
    }
}