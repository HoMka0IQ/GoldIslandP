using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCellData : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public void SetData(Color color, string text)
    {
        this.text.text = text;
        this.text.color = color;
    }

}
