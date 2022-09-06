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
        //await DungeonMerchant.FileIO.JsonSerializationHandler.DeleteSave("SaveData.json");
        var musicVol = PlayerPrefs.GetFloat("Music Volume");
        var sfxVol = PlayerPrefs.GetFloat("SFX Volume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Music Volume", musicVol);
        PlayerPrefs.SetFloat("SFX Volume", sfxVol);


    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    PlayerPrefs.DeleteAll();
        //}
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
    public void SaveGame()
    {
        var newSave = new SaveData();
        var allHeroes = GameObject.FindObjectsOfType<Stats>();
        var allHeroesData = new List<HeroData>();
        foreach (Stats s in allHeroes)
        {
            s.StoreData();
            allHeroesData.Add(s.Data);
        }
        newSave.Gold = Stock.Gold;
        newSave.MaxProfit = Stock.MaxProfit;
        newSave.MaxLevel = Hero.MaxDungeonFloor;
        var allItems = GameObject.FindObjectsOfType<Item>();
        var allItemsData = new List<ItemData>();
        foreach (Item i in allItems)
        {
            i.StoreData();
            allItemsData.Add(i.Data);
        }
        

        var allRequests = GameObject.FindObjectsOfType<Request>();
        var allRequestData = new List<RequestData>();
        foreach(Request r in allRequests)
        {
            r.StoreData();
            allRequestData.Add(r.Data);
        }

        Dungeon.StoreData();
        var newDungeonSaveData = Dungeon.SaveData;
        Inn.StoreData();
        var newInnSaveData = Inn.Data;

        newSave.AllHeroes = allHeroesData;
        newSave.InnSaveData = newInnSaveData;
        newSave.DungeonSaveData = newDungeonSaveData;
        newSave.AllItems = allItemsData;
        newSave.AllRequests = allRequestData;
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
            foreach (HeroData hd in loadedData.AllHeroes)
            {
                if (hd.Hired)
                {
                    HeroGen.CreateSpecificHero(hd);

                }


            }
            Stock.Gold = loadedData.Gold;
            Stock.MaxProfit = loadedData.MaxProfit;
            Hero.MaxDungeonFloor = loadedData.MaxLevel;
            Stock.GoldText.text = Stock.Gold + "G";

            foreach (ItemData id in loadedData.AllItems)
            {
                if (!id.Equipped && !id.Merchant)
                {
                    ItemGen.CreateSpecificItem(id);
                }
                if (id.Merchant)
                {
                    ItemGen.CreateSpecificItem(id);
                }
                
            }

            
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
