using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocalTouchChecker : MonoBehaviour
{
    [SerializeField] GraphicRaycaster uiRaycaster;
    [SerializeField] EventSystem eventSystem;

    [SerializeField] int currentTouches;
    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }
    public int CheckTouches()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> result = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerData, result);

            Debug.Log("Number of UI elements under mouse click s: " + result.Count);
            currentTouches = result.Count;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = touch.position
            };

            List<RaycastResult> result = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerData, result);

            Debug.Log("Number of UI elements under touch s: " + result.Count);
            currentTouches = result.Count;
        }
        return currentTouches;
    }
}
