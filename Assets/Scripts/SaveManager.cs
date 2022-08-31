using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class SaveManager : MonoBehaviour
{
    public LevelManager Level;
    public Button ContinueButton;
    public StockManager Stock;
    public EquipMenu Equipment;
    public ItemGenerator ItemGen;
    public HeroGenerator HeroGen;
    public bool MainGame = false;
    public HeroManager Hero;

    

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
        File.Delete(Application.persistentDataPath + "/SaveGame.json");
        //await DungeonMerchant.FileIO.JsonSerializationHandler.DeleteSave("SaveData.json");
        var musicVol = PlayerPrefs.GetFloat("Music Volume");
        var sfxVol = PlayerPrefs.GetFloat("SFX Volume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Music Volume", musicVol);
        PlayerPrefs.SetFloat("SFX Volume", sfxVol);


    }

    //public async void SaveGame()
    //{
    //    var newSave = new SaveData();
    //    newSave.Gold = Stock.Gold;
    //    newSave.MaxProfit = Stock.MaxProfit;
    //    newSave.MaxLevel = Hero.MaxDungeonFloor;

    //    var allItems = GameObject.FindObjectsOfType<Item>();
    //    var allHeroes = GameObject.FindObjectsOfType<Stats>();
    //    var allItemsData = new List<ItemData>();
    //    var allHeroesData = new List<HeroData>();
    //    foreach(Item i in allItems)
    //    {
    //        i.StoreData();
    //        allItemsData.Add(i.Data);
    //    }
    //    foreach (Stats s in allHeroes)
    //    {
    //        s.StoreData();
    //        allHeroesData.Add(s.Data);
    //    }
    //    newSave.AllItems = allItemsData;
    //    newSave.AllHeroes = allHeroesData;
    //    await DungeonMerchant.FileIO.JsonSerializationHandler.ResolveDataDirectoryAsync();
    //    await DungeonMerchant.FileIO.JsonSerializationHandler.SerializeObjectToDataDirectory(newSave, "SaveData.json");


    //    Debug.Log("Did the save thing");
    //}
    public async void SaveGame()
    {
        var newSave = new SaveData();
        newSave.Gold = Stock.Gold;
        newSave.MaxProfit = Stock.MaxProfit;
        newSave.MaxLevel = Hero.MaxDungeonFloor;

        var allItems = GameObject.FindObjectsOfType<Item>();
        var allHeroes = GameObject.FindObjectsOfType<Stats>();
        var allItemsData = new List<ItemData>();
        var allHeroesData = new List<HeroData>();
        foreach (Item i in allItems)
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
        string saveData = JsonUtility.ToJson(newSave);
        string savePath;
        savePath = Application.persistentDataPath + "/SaveGame.json";

        File.WriteAllText(savePath, saveData);


        Debug.Log("Did the save thing");

        
    }

    public void LoadGame()
    {

        //if(!await DungeonMerchant.FileIO.JsonSerializationHandler.CheckIfFileExistsInDataDirectory("SaveData.json"))
        //{
        //    Debug.Log("NO SAVE FILE");
        //    return;

        //}
        //Debug.Log("Loading");
        //SaveData loadedData = await DungeonMerchant.FileIO.JsonSerializationHandler.DeserializeObjectFromDataDirectory<SaveData>("SaveData.json");
        string savePath;
        savePath = Application.persistentDataPath + "/SaveGame.json";

        if (File.Exists(savePath))
        {
            string fileContents = File.ReadAllText(savePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(fileContents);
            Stock.Gold = loadedData.Gold;
            Stock.MaxProfit = loadedData.MaxProfit;
            Hero.MaxDungeonFloor = loadedData.MaxLevel;
            Stock.GoldText.text = Stock.Gold + "G";
            foreach (ItemData id in loadedData.AllItems)
            {
                if (!id.Merchant && !id.Equipped)
                {
                    ItemGen.CreateSpecificItem(id);
                }
            }

            foreach (HeroData hd in loadedData.AllHeroes)
            {
                if (hd.Hired)
                {
                    HeroGen.CreateSpecificHero(hd);

                }


            }

            Stock.UpdatePrices();
        }

        

    }





}
