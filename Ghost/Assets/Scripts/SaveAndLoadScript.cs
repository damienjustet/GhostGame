using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
[System.Serializable]
public class SaveAndLoadScript : MonoBehaviour
{
    
    public float Money;
   public static void SaveGame(float Money)
    {
       SaveLoadData data = new SaveLoadData();
       data.moneys = Money;
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, "savefile.json");
        File.WriteAllText(path, json);
        print("Save: " + Money);
    }
    public static float LoadGame()
    {
        
         string path = Path.Combine(Application.persistentDataPath, "savefile.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveLoadData moneyLoad = JsonUtility.FromJson<SaveLoadData>(json);
            print("load: " + moneyLoad);
            return moneyLoad.moneys;
            

        }
        else
        {
            print("file not found");
            return 0.0f;
            
        }
    }
}
