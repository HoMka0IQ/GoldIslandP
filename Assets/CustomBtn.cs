using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomBtn : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public UnityEvent btnAction;
    [SerializeField] float sizeChanger = 0.9f;
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector2(sizeChanger, sizeChanger);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = new Vector2(1, 1);
        btnAction.Invoke();
    }
}
