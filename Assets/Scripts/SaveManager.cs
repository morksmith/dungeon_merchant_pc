using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveManager : MonoBehaviour
{
    public LevelManager Level;
    public Button ContinueButton;


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
        var newSave = new SaveData();
        var allItems = GameObject.FindObjectsOfType<Item>();
        var stockItems = new List<ItemData>();
        foreach(Item i in allItems)
        {
            if(!i.Merchant && !i.Equipped)
            {
                stockItems.Add(i.Data);
            }
        }
        newSave.stockItemData = stockItems;
        await DungeonMerchant.FileIO.JsonSerializationHandler.ResolveDataDirectoryAsync();
        await DungeonMerchant.FileIO.JsonSerializationHandler.SerializeObjectToDataDirectory(newSave, "SaveData.json");

        Debug.Log("Did the save thing");
    }
    




}
