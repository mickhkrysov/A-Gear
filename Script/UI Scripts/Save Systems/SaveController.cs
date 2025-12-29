using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject savedTextObject;
    public GameObject resetTextError;   //shows if user wants to reset progress but he didn't save before
    public GameObject resetProgressSucsess;     //shows if progress been reseted

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
    //shows that progress is saved
    public void ShowTextButton()
    {
        savedTextObject.SetActive(true);
        resetTextError.SetActive(false);
        resetProgressSucsess.SetActive(false);
    }

    //resets the saved data
    public void ResetSavedData()
    {
        if (File.Exists(saveLocation))          //check later if i can do switch
        {
            File.Delete(saveLocation);
            resetProgressSucsess.SetActive(true);
            resetTextError.SetActive(false);
            savedTextObject.SetActive(false);
        }

        else
        {
            resetTextError.SetActive(true);
            resetProgressSucsess.SetActive(false);
            savedTextObject.SetActive(false);
        }
    }

}
