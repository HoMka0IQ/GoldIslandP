using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IncorrectActionAnim : MonoBehaviour
{
    public GameObject massagePrefab;
    public static IncorrectActionAnim instance;
    IncorrectMassage currentMassage;

    private void Awake()
    {
        instance = this;
    }

    public void ShowMassage(string massage, Vector3 worldPos)
    {
        currentMassage = Instantiate(massagePrefab, transform).GetComponent<IncorrectMassage>();
        currentMassage.SetData(massage, worldPos);
    }

    
}
