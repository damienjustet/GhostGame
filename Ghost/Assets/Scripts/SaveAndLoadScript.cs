using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
[System.Serializable]
public class SaveAndLoadScript
{
    
    public float Money;
   public static void SaveGame(float Money)
    {
        string json = JsonUtility.ToJson(Money, true);
        string path = Path.Combine(Application.persistentDataPath, "savefile.json");
        File.WriteAllText(path, json);
    }
    public static float LoadGame()
    {
         string path = Path.Combine(Application.persistentDataPath, "savefile.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            float moneyLoad = JsonUtility.FromJson<float>(json);
            return moneyLoad;
        }
        else
        {
            Debug.Log("File Not Found");
            return 0.0f;
        }
    }
}
