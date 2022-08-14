using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveManager : MonoBehaviour
{
    public LevelManager Level;
    public Button ContinueButton;
    public SaveData Data;
    public List<Item> StockItems;


    private void Start()
    {

        if(PlayerPrefs.GetFloat("Tutorial Complete") == 0)
        {
            if (ContinueButton != null)
            {
                ContinueButton.interactable = false;
            }
        }
        else
        {
            if (ContinueButton != null)
            {
                ContinueButton.interactable = true;
            }
        }

    }

    public void ResetProgress()
    {
        var musicVol = PlayerPrefs.GetFloat("Music Volume");
        var sfxVol = PlayerPrefs.GetFloat("SFX Volume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Music Volume", musicVol);
        PlayerPrefs.SetFloat("SFX Volume", sfxVol);

    }

    public async void SaveGame()
    {
        Data.CollectData();
        StockItems = Data.StockItems;
        DungeonMerchant.FileIO.JsonSerializationHandler.ResolveDataDirectoryAsync();
        DungeonMerchant.FileIO.JsonSerializationHandler.SerializeObjectToDataDirectory(StockItems, "SaveData.json");

        Debug.Log("Did the save thing");
    }
    




}
