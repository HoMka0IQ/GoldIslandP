using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IslandSavesData
{
    public int id;
    public int typeID;
    public string[] cellsName = new string[16];
    public float[] position = new float[3];

    public List<int> factoriesID = new List<int>();
    public List<int> factoriesLVL = new List<int>();

    public float[] naturalCellRemoveTimer = new float[16];
    public bool[] naturalCellRemoveProcessStart = new bool[16];
    public IslandSavesData(IslandData islandData)
    {
        id = islandData.islandPosInArray;
        typeID = islandData.islandTypeID;
        for (int i = 0; i < islandData.GetCellsSO().Length; i++)
        {
            //Debug.Log(islandData.GetCells()[i].cellName);
            cellsName[i] = islandData.GetCellsSO()[i].cellName;

            // Спробуємо привести об'єкт до FactoryCell
            FactoryCell factoryCell = islandData.GetCells()[i] as FactoryCell;
            if (factoryCell != null)
            {
                factoriesID.Add(i);
                factoriesLVL.Add(factoryCell.currentLVL);
            }

            // Спробуємо привести об'єкт до NaturalCell
            NaturalCell naturalCell = islandData.GetCells()[i] as NaturalCell;
            if (naturalCell != null)
            {

                if (naturalCell.removeProcess == true)
                {
                    Debug.Log("Зберігання клітинки з індексом " + i + " типу natural");
                    naturalCellRemoveTimer[i] = naturalCell.removeTimer;
                    naturalCellRemoveProcessStart[i] = naturalCell.removeProcess;

                }
                
            }

        }
        position[0] = islandData.gameObject.transform.position.x;
        position[1] = islandData.gameObject.transform.position.y;
        position[2] = islandData.gameObject.transform.position.z;
    }
}
