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
    public DungeonManager Dungeon;
    public ItemGenerator ItemGen;
    public HeroGenerator HeroGen;
    public bool MainGame = false;
    public HeroManager Hero;
    public InnManager Inn;
    public RequestManager Requests;

    

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
        var musicVol = PlayerPrefs.GetFloat("Music Volume");
        var sfxVol = PlayerPrefs.GetFloat("SFX Volume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Music Volume", musicVol);
        PlayerPrefs.SetFloat("SFX Volume", sfxVol);


    }
    
    public void SaveGame()
    {
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


        //Save Gold and Profit
        newSave.Gold = Stock.Gold;
        newSave.MaxProfit = Stock.MaxProfit;
        newSave.MaxLevel = Hero.MaxDungeonFloor;

        //Save Items
        var allItems = GameObject.FindObjectsOfType<Item>();
        var allItemsData = new List<ItemData>();
        foreach (Item i in allItems)
        {
            i.StoreData();
            allItemsData.Add(i.Data);
        }
        newSave.AllItems = allItemsData;

        //Save current requests
        var allRequests = GameObject.FindObjectsOfType<Request>();
        var allRequestData = new List<RequestData>();
        foreach(Request r in allRequests)
        {
            r.StoreData();
            allRequestData.Add(r.Data);
        }
        newSave.AllRequests = allRequestData;

        //Save Dungeon State
        Dungeon.StoreData();
        var newDungeonSaveData = Dungeon.SaveData;
        newSave.DungeonSaveData = newDungeonSaveData;


        //Save Inn State
        Inn.StoreData();
        var newInnSaveData = Inn.Data;
        newSave.InnSaveData = newInnSaveData;

        //Save to file
        string saveData = JsonUtility.ToJson(newSave);
        string savePath;
        savePath = Application.persistentDataPath + "/SaveGame.json";
        File.WriteAllText(savePath, saveData);


        Debug.Log("Did the save thing");

        
    }

    public void LoadGame()
    {

      
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

            if(loadedData.InnSaveData != null)
            {
                Inn.SetTimers(loadedData.InnSaveData.MerchTime, loadedData.InnSaveData.HeroTime, loadedData.InnSaveData.RequestTime);
                if (loadedData.InnSaveData.HeroAvailable)
                {
                    Inn.Hire.LoadHireHero(loadedData.InnSaveData.HireHero);
                }
                
            }
            var loadedRequests = new List<RequestData>();
            foreach(RequestData rd in loadedData.AllRequests)
            {
                loadedRequests.Add(rd);
            }

            Requests.CreateSpecificRequests(loadedRequests.ToArray());
            
           

            Stock.UpdatePrices();
        }
       
        

    }





}
