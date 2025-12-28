using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //define save location
        saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");

        LoadGame();
    }

    // class to save data
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    // load data
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.PlayerPosition;
        }
        else
        {
            SaveGame();
        }
    }
}
