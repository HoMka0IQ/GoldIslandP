using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCellData : MonoBehaviour
{
    [SerializeField] Material buffMat;
    [SerializeField] Material debuffMat;

    [SerializeField] MeshRenderer MRBorder;
    [SerializeField] MeshRenderer MRZone;
    public void setData(Material zoneMat)
    {
        MRZone.material = zoneMat;
        MRBorder.material = zoneMat;
    }
}
