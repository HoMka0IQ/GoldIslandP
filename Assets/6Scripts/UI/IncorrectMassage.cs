using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

// This class is used to show a message from IncorrectActionAnim class
public class IncorrectMassage : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public GameObject center;

    [SerializeField] ConnectionScreenToWorld connectionScreenToWorld;
    public void SetData(string massage, Vector3 pos)
    {
        text.text = massage;
        center.transform.DOLocalMove(center.transform.localPosition + new Vector3(0, 200, 0), 3f).OnComplete(() => Destroy(gameObject));
        connectionScreenToWorld.setData(pos);
    }

}
