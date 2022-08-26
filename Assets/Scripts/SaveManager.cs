using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveManager : MonoBehaviour
{
    public LevelManager Level;
    public Button ContinueButton;
    public StockManager Stock;
    public EquipMenu Equipment;
    public ItemGenerator ItemGen;
    public HeroGenerator HeroGen;
    public bool MainGame = false;


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

        if (MainGame)
        {
            LoadGame();
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
        var allHeroes = GameObject.FindObjectsOfType<Stats>();
        var allItemsData = new List<ItemData>();
        var allHeroesData = new List<HeroData>();
        foreach(Item i in allItems)
        {
            i.StoreData();
            allItemsData.Add(i.Data);
        }
        foreach (Stats s in allHeroes)
        {
            s.StoreData();
            allHeroesData.Add(s.Data);
        }
        newSave.AllItems = allItemsData;
        newSave.AllHeroes = allHeroesData;
        await DungeonMerchant.FileIO.JsonSerializationHandler.ResolveDataDirectoryAsync();
        await DungeonMerchant.FileIO.JsonSerializationHandler.SerializeObjectToDataDirectory(newSave, "SaveData.json");

        Debug.Log("Did the save thing");
    }

    public async void LoadGame()
    {
        if(!await DungeonMerchant.FileIO.JsonSerializationHandler.CheckIfFileExistsInDataDirectory("SaveData.json"))
        {
            return;
        }
        SaveData loadedData = await DungeonMerchant.FileIO.JsonSerializationHandler.DeserializeObjectFromDataDirectory<SaveData>("SaveData.json");
        foreach(ItemData id in loadedData.AllItems)
        {
            if(!id.Merchant && !id.Equipped)
            {
                ItemGen.CreateSpecificItem(id);
            }
        }

        foreach(HeroData hd in loadedData.AllHeroes)
        {
            if (hd.Hired)
            {
                HeroGen.CreateSpecificHero(hd);
                
            }
            

        }

        Stock.UpdatePrices();

    }





}
