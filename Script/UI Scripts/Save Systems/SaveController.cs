using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    public GameObject savedTextObject;
    public GameObject resetTextError;   //shows if user wants to reset progress but he didn't save before
    public GameObject resetProgressSucsess;     //shows if progress been reseted
    private Chest[] chests;

    void Start()
    {
        InitializeComponents();
        LoadGame();
    }


    private void InitializeComponents()
    {
       //define save location
        saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");
        inventoryController = FindFirstObjectByType<InventoryController>();
        chests = FindObjectsByType<Chest>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    }

    // class to save data
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventoryController.GetInventoryItems(),
            chestSaveData = GetChestsState()
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));

    }


    private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();
        foreach (var chest in chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.ChestID,
                isOpened = chest.isOpen
            };
            chestStates.Add(chestSaveData);
        }
        return chestStates;
    }

    // load data
        public void LoadGame()
    {
        if (!File.Exists(saveLocation))
        {
            SaveGame();
            return;
        }

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

        GameObject.FindGameObjectWithTag("Player").transform.position = saveData.PlayerPosition;

        inventoryController.SetInventoryItems(saveData.inventorySaveData);
        LoadChestStates(saveData.chestSaveData);
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData =
                chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if (chestSaveData != null)
                chest.SetOpened(chestSaveData.isOpened);
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
