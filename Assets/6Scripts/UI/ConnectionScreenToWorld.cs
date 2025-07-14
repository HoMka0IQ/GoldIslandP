using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionScreenToWorld : MonoBehaviour
{
    [SerializeField] GameObject targetGO;
    public Vector3 targetPos = new Vector3();
    public Vector3 moveFromCenter;
    Camera cam;
    bool attachToGo;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (attachToGo)
        {
            transform.position = cam.WorldToScreenPoint(targetGO.transform.position + moveFromCenter);
            return;
        }
        transform.position = cam.WorldToScreenPoint(targetPos + moveFromCenter);
    }
    public void setData(Vector3 pos)
    {
        targetPos = pos;
        attachToGo = false;
    }
    public void setData(GameObject go)
    {
        targetGO = go;
        attachToGo = true;  
    }
}
