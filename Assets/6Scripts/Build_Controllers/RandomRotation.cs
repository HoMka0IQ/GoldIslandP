using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] GameObject go;


    private void Start()
    {
        go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
}
