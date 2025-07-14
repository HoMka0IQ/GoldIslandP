using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.XR;

public class CalcPathInWater : MonoBehaviour
{
    public float timer;
    float _timer;

    public GameObject chestPrefab;
    GameObject chest;
    public Vector3[] corners = new Vector3[4];

    public static CalcPathInWater instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _timer = timer;
    }
    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                if (chest == null)
                {
                    chest = Instantiate(chestPrefab);
                }
                FindFreeWay();
                chest.SetActive(true);
                int rand = Random.Range(0, corners.Length - 1);
                chest.transform.position = corners[rand] + new Vector3(0,2.5f,0);
                chest.transform.DOMove(corners[rand + 1] + new Vector3(0, 2.5f, 0), 65f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    chest.transform.DOMove(chest.transform.position + new Vector3(0, -10, 0),10f).OnComplete(() =>
                    {
                        chest.SetActive(false);
                    }); ;
                });
                _timer = timer;

            }
            
        }
    }
    public void FindFreeWay()
    {
        List<Vector3> allEmptyPos = IslandBuilding.Instance.GetEmptyPos();
        if (allEmptyPos == null || allEmptyPos.Count == 0)
            return;

        float minX = float.MaxValue, maxX = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        foreach (var pos in allEmptyPos)
        {
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.z < minZ) minZ = pos.z;
            if (pos.z > maxZ) maxZ = pos.z;
        }

        corners[0] = new Vector3(minX, 0, minZ);
        corners[1] = new Vector3(maxX, 0, minZ);
        corners[2] = new Vector3(maxX, 0, maxZ);
        corners[3] =new Vector3(minX, 0, maxZ);


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);
    }

}