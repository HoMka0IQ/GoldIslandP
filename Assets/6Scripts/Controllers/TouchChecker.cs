using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchChecker : MonoBehaviour
{
    [SerializeField] bool isTouching = false;
    [SerializeField] bool isTrueTouching = false;
    [SerializeField] bool isMoving = false;
    [SerializeField] Vector2 initialTouchPosition;

    public static TouchChecker instance;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(0))
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    initialTouchPosition = touch.position;
                    isTouching = true;
                    isMoving = false;
                    isTrueTouching = false;
                    break;

                case TouchPhase.Moved:
                    isTouching = false;
                    isMoving = true;
                    break;

                case TouchPhase.Ended:
                    isTouching = false;
                    if (isMoving == false)
                    {
                        isTrueTouching = true;
                    }
                    isMoving = false;
                    break;
            }
        }
    }
    public bool IsPointerOverUIObject(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
    public bool GetTrueTouch()
    {
        return isTrueTouching;
    }
    public bool GetTrueMove()
    {
        return isMoving;
    }
}
