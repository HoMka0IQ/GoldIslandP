using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectorUI : MonoBehaviour
{
    public GameObject UI;
    public ConnectionScreenToWorld connectionScreenToWorld;
    public TMP_Text collectMoneyText;

    public Action OnCollectValue;
    public CollectorUI SetTarget(GameObject target)
    {
        connectionScreenToWorld.setData(target);
        return this;
    }
    public void SetActive(bool action)
    {
        UI.SetActive(action);
    }
    public void UpdateText(string text)
    {
        collectMoneyText.text = text;
    }
    public void CollectValue()
    {
        OnCollectValue.Invoke();
    }

}
