using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    public LevelManager Level;
    public Button ContinueButton;
    public StockManager Stock;
    public EquipMenu Equipment;
    public DungeonManager Dungeon;
    public ItemGenerator ItemGen;
    public HeroGenerator HeroGen;
    public bool MainGame = false;
    public HeroManager Hero;
    public InnManager Inn;
    public RequestManager Requests;
    public Menu WarningMenu;
    public Prospector Prospector;
    public MagicChest MagicChest;

    public bool ItemsSaved = false;
    public bool HeroesSaved = false;
    public bool StockSaved = false;
    public bool DungeonSaved = false;
    public bool InnSaved = false;
    public bool RequestsSaved = false;




    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Start()
    {
        

        if (PlayerPrefs.GetFloat("Tutorial Complete") == 0)
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

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (!focus)
    //    {
    //        SaveGame();
    //    }
    //}

    public void ResetProgress()
    {
        File.Delete(Application.persistentDataPath + "/SaveGame.json");
        var musicVol = PlayerPrefs.GetFloat("Music Volume");
        var sfxVol = PlayerPrefs.GetFloat("SFX Volume");
        var survivalMode = PlayerPrefs.GetInt("Survival Mode");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Music Volume", musicVol);
        PlayerPrefs.SetFloat("SFX Volume", sfxVol);
        PlayerPrefs.SetFloat("Survival Mode", survivalMode);
        


    }
    
    public void SaveGame()
    {

        //Set all save checks to false
        ItemsSaved = false;
        HeroesSaved = false;
        DungeonSaved = false;
        InnSaved = false;
        StockSaved = false;
        RequestsSaved = false;


        //Save Heroes
        var newSave = new SaveData();
        var allHeroes = GameObject.FindObjectsOfType<Stats>();
        var allHeroesData = new List<HeroData>();
        foreach (Stats s in allHeroes)
        {
            s.StoreData();
            allHeroesData.Add(s.Data);
        }
        newSave.AllHeroes = allHeroesData;
        HeroesSaved = true;

        //Save Gold and Profit
        newSave.Gold = Stock.Gold;
        newSave.MaxProfit = Stock.MaxProfit;
        newSave.MaxLevel = Hero.MaxDungeonFloor;
        StockSaved = true;

        //Save Items
        var allItems = GameObject.FindObjectsOfType<Item>();
        var allItemsData = new List<ItemData>();
        foreach (Item i in allItems)
        {
            i.StoreData();
            allItemsData.Add(i.Data);
        }
        newSave.AllItems = allItemsData;
        ItemsSaved = true;

        //Save current requests
        var allRequests = GameObject.FindObjectsOfType<Request>();
        var allRequestData = new List<RequestData>();
        foreach(Request r in allRequests)
        {
            r.StoreData();
            allRequestData.Add(r.Data);
        }
        newSave.AllRequests = allRequestData;
        RequestsSaved = true;

        //Save Dungeon State
        Dungeon.StoreData();
        var newDungeonSaveData = Dungeon.SaveData;
        newSave.DungeonSaveData = newDungeonSaveData;
        DungeonSaved = true;


        //Save Inn State
        Inn.StoreData();
        var newInnSaveData = Inn.Data;
        newSave.InnSaveData = newInnSaveData;
        InnSaved = true;

        //Save Chests
        var allChests = GameObject.FindObjectsOfType<Chest>();
        var allChestData = new List<ChestData>();
        foreach (Chest c in allChests)
        {
            c.StoreData();
            allChestData.Add(c.Data);
        }
        newSave.AllChests = allChestData;

        //Save timestamp for AFK stats

        DateTime currentDateTime = System.DateTime.Now;
        Debug.Log(currentDateTime.ToString("G"));
        PlayerPrefs.SetString("DateTime", currentDateTime.ToString("G"));
        Prospector.StoreData();
        newSave.ProspectorData = Prospector.Data;

        //Save Magic Chest Data
        MagicChest.StoreData();
        newSave.MagicChestData = MagicChest.Data;
        

        //Save to file
        string saveData = JsonUtility.ToJson(newSave);
        string savePath;
        savePath = Application.persistentDataPath + "/SaveGame.json";
        File.WriteAllText(savePath, saveData);
        Debug.Log("Did the save thing");

        
    }

    public void LoadGame()
    {
        if(PlayerPrefs.GetInt("Closed Correctly") != 1)
        {
            WarningMenu.Activate();
        }
      
        string savePath;
        savePath = Application.persistentDataPath + "/SaveGame.json";

        if (File.Exists(savePath))
        {
            string fileContents = File.ReadAllText(savePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(fileContents);

            //Load Heroes
            foreach (HeroData hd in loadedData.AllHeroes)
            {
                if (hd.Hired)
                {
                    HeroGen.CreateSpecificHero(hd);
                }
            }

            //Load Gold and Profit
            Stock.Gold = loadedData.Gold;
            Stock.MaxProfit = loadedData.MaxProfit;
            Hero.MaxDungeonFloor = loadedData.MaxLevel;
            Stock.GoldText.text = Stock.Gold + "G";

            //Load Items
            foreach (ItemData id in loadedData.AllItems)
            {
                if (id.Merchant)
                {
                    ItemGen.CreateSpecificItem(id);
                }
                else if (!id.Equipped)
                {
                    ItemGen.CreateSpecificItem(id);
                }
            }

            //Load Dungeon State
            if(loadedData.DungeonSaveData != null)
            {
                Dungeon.CurrentTime = loadedData.DungeonSaveData.Time;
                Dungeon.EnemyCount = Mathf.Clamp(loadedData.DungeonSaveData.EnemyCount, 1, 3);
                Dungeon.NextCount = Mathf.Clamp(loadedData.DungeonSaveData.NextCount, 1, 3);
                Dungeon.EnemyStrength = Mathf.Clamp(loadedData.DungeonSaveData.EnemyStrength,1,3);
                Dungeon.NextStrength = Mathf.Clamp(loadedData.DungeonSaveData.NextStrength,1,3);
                var typeList = new List<int>();
                foreach(int i in loadedData.DungeonSaveData.SpawnTypes)
                {
                    typeList.Add(i);
                }
                if(typeList.Count > 0)
                {
                    Dungeon.SpawnTypes = typeList.ToArray();
                }
                
                Dungeon.SleepTimer = loadedData.DungeonSaveData.SleepTimer;
                Dungeon.UpdateUI();
            }

            //Load Chests
            if(loadedData.AllChests.Count > 0)
            {
                foreach(ChestData cd in loadedData.AllChests)
                {
                    Stock.AddChest(cd.Type, cd.Level);
                }
            }

            //Load Inn Data
            if(loadedData.InnSaveData != null)
            {
                Inn.SetTimers(loadedData.InnSaveData.MerchTime, loadedData.InnSaveData.HeroTime, loadedData.InnSaveData.RequestTime);
                if (loadedData.InnSaveData.HeroAvailable)
                {
                    Inn.Hire.LoadHireHero(loadedData.InnSaveData.HireHero);
                }                
            }

            //Load Requests
            var loadedRequests = new List<RequestData>();
            foreach(RequestData rd in loadedData.AllRequests)
            {
                loadedRequests.Add(rd);
            }
            Requests.CreateSpecificRequests(loadedRequests.ToArray());

            //Load Prospector
            if(Prospector!= null)
            {
                Prospector.IsHired = loadedData.ProspectorData.IsHired;
                Prospector.Timer = loadedData.ProspectorData.Timer;
                Prospector.Mining = loadedData.ProspectorData.Mining;
                Prospector.ReturnedFromMining = loadedData.ProspectorData.ReturnedFromMining;               
                Prospector.CurrentLevel = loadedData.ProspectorData.CurrentLevel;
                Prospector.ProspectTime = loadedData.ProspectorData.ProspectTime;
                Prospector.CheckTimePassed();
            }
            

            //Load Magic Chest
            if(MagicChest != null)
            {
                MagicChest.Timer = loadedData.MagicChestData.Timer;
                MagicChest.Restocking = loadedData.MagicChestData.Restocking;
                MagicChest.ReadyToCollect = loadedData.MagicChestData.ReadyToCollect;
                MagicChest.RestockTime = loadedData.MagicChestData.RestockTime;
                MagicChest.CheckTimePassed();
            }


            Debug.Log("Loaded Successfully");
            PlayerPrefs.SetInt("Closed Correctly", 0);
            Stock.UpdatePrices();
        }
       
        

    }





}
