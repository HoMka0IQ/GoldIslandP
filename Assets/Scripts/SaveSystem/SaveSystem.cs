using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFolder = Application.dataPath + "/Saves/";

    static SaveSystem()
    {
        // Створюємо папку, якщо її нема
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
    }

    //PLAYER DATA SAVES
    public static void SavePlayerData(GameData gameData)
    {
        string path = saveFolder + "PlayerSaves.json";
        PlayerData playerData = new PlayerData(gameData);

        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, json);
    }

    public static PlayerData LoadPlayerData()
    {
        string path = saveFolder + "PlayerSaves.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    //SAVE ISLAND
    public static void SaveIsland(List<IslandData> islandsData)
    {
        string path = saveFolder + "IslandSaves.json";
        List<IslandSavesData> dataList = new List<IslandSavesData>();
        foreach (var islandData in islandsData)
        {
            dataList.Add(new IslandSavesData(islandData));
        }
        string json = JsonUtility.ToJson(new SerializationWrapper<List<IslandSavesData>>(dataList));
        File.WriteAllText(path, json);
        Debug.Log("Save Data");
    }

    public static List<IslandSavesData> LoadIsland()
    {
        string path = saveFolder + "IslandSaves.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SerializationWrapper<List<IslandSavesData>> dataList = JsonUtility.FromJson<SerializationWrapper<List<IslandSavesData>>>(json);
            Debug.Log("Load Data");
            return dataList.value;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T value;
        public SerializationWrapper(T value) { this.value = value; }
    }
}
