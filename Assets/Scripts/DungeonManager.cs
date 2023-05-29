using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonManager : MonoBehaviour
{
    public bool Tutorial = false;
    public bool SurvivalMode = false;
    public bool Paused = false;
    public InnManager Inn;
    public bool Running = false;
    public GameObject HeroUI;
    public StockManager Stock;
    public float CurrentTime;
    public float Minutes;
    public float Hours;
    public int Level = 1;
    public int EnemyCount = 1;
    public float GoldBonus = 1;
    public int NextCount;
    public Vector3 HeroStartPosition;
    public Slider EnemyCountSlider;
    public Slider EnemyStrengthSlider;
    public Sprite UpSprite;
    public Sprite DownSprite;
    public Image CountTrendIcon;
    public Image StrengthTrendIcon;
    public int EnemyStrength = 1;
    public int NextStrength;
    public int[] SpawnTypes;
    public Image[] SpawnIcons;
    public Sprite[] TypeSprites;
    public float MaxTime = 1440;
    public Slider TimeSlider;
    public Image HeroImage;
    public Sprite HandSprite;
    public Image ConsumableIcon;
    public TextMeshProUGUI ConsumableText;
    public SpriteRenderer HeroSprite;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI DungeonText;
    public TextMeshProUGUI HeroLevel;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI LootText;
    public TextMeshProUGUI LevelText;
    public Stats CurrentHeroStats;
    public HeroAI CurrentHeroAI;
    public Slider HeroHP;
    public Slider HeroXP;
    public EnemySpawner[] EnemySpawners;
    public Prop[] Props;
    public bool DungeonCompleted = false;
    public Menu DungeonCompleteMenu;
    public TextMeshProUGUI CompleteTitle;
    public TextMeshProUGUI CompleteText;
    public TextMeshProUGUI GoldCollectedText;
    public TextMeshProUGUI LootCollectedText;
    public TextMeshProUGUI GoldMultiplierText;
    public GameObject RIPButton;
    public GameObject CompleteButtons;
    public Image CompleteImage;
    public Sprite DeadSprite;
    public Sprite CompleteSprite;
    public List<GameObject> DungeonLayouts;
    public Button SleepButton;
    public Slider SleepSlider;
    public float SleepTimer;
    public float SleepTime;
    public ScrollingWindow TopContent;
    public AudioClip EnterDungeonSound;
    public AudioClip DungeonCompleteSound;
    public AudioClip NewDaySound;
    public SaveManager Save;
    public DungeonData SaveData;
    public Menu PlayerUpgradeMenu;
    private bool readyForNextDungeon = false;
    private float nextDungeonTime = 1;
    private float timer;
    private float levelCount = 0;
    private bool newHighScore = false;

    private SFXManager sfx;

    // Start is called before the first frame update
    void Start()
    {
        sfx = GameObject.FindObjectOfType<SFXManager>();
        EnemySpawners = GameObject.FindObjectsOfType<EnemySpawner>();
        Props = GameObject.FindObjectsOfType<Prop>();
        
        if (Tutorial)
        {
            EnemyCount = 2;
            NextCount = 3;
            EnemyStrength = 2;
            NextStrength = 3;
            EnemyCountSlider.value = EnemyCount * 0.333f + 0.04f;
            EnemyStrengthSlider.value = EnemyStrength * 0.333f + 0.04f;
        }

        
        
        

        SetEnemyTypes();
        UpdateUI();
        Stock.UpdatePrices();


    }

    // Update is called once per frame
    void Update()
    {
        if (readyForNextDungeon)
        {
            timer += Time.deltaTime;
            if(timer > nextDungeonTime)
            {
                readyForNextDungeon = false;
                SendToNextLevel();
            }
        }

        if (SurvivalMode)
        {
            CurrentHeroAI.gameObject.SetActive(true);
            HeroHP.value = CurrentHeroStats.HP / CurrentHeroStats.MaxHP;
            HeroXP.value = CurrentHeroStats.XP / CurrentHeroStats.MaxXP;
            HeroLevel.text = CurrentHeroStats.Level.ToString();
            GoldText.text = CurrentHeroStats.GoldHeld.ToString();
            LootText.text = CurrentHeroStats.LootHeld.ToString();
            SleepButton.gameObject.SetActive(false);
           
        }

        if (Paused)
        {
            return;
        }
        if (CurrentTime < MaxTime)
        {
            CurrentTime += Time.deltaTime * 12;
        }
        else
        {
            CycleDungeon();
        }
        


        Hours = (CurrentTime / 60) % 24;
        Minutes = (CurrentTime % 60);

        Hours = Mathf.FloorToInt(Hours);
        Minutes = Mathf.FloorToInt(Minutes);

        string hourDisplay = Hours.ToString("00");
        string minuteDisplay = Minutes.ToString("00");

        TimeText.text = hourDisplay + ":" + minuteDisplay;
        TimeSlider.value = CurrentTime / MaxTime;

        if (CurrentHeroStats != null)
        {
            CurrentHeroAI.gameObject.SetActive(true);
            HeroHP.value = CurrentHeroStats.HP / CurrentHeroStats.MaxHP;
            HeroXP.value = CurrentHeroStats.XP / CurrentHeroStats.MaxXP;
            HeroLevel.text = CurrentHeroStats.Level.ToString();
            GoldText.text = CurrentHeroStats.GoldHeld.ToString();
            LootText.text = CurrentHeroStats.LootHeld.ToString();
        }
        else
        {
            HeroUI.SetActive(false);
            SleepButton.gameObject.SetActive(true);
            CurrentHeroAI.gameObject.SetActive(false);
        }

        if (!Running)
        {
            if (SleepTimer < SleepTime)
            {
                SleepTimer += Time.deltaTime;
                SleepButton.interactable = false;
                SleepSlider.gameObject.SetActive(true);
                SleepSlider.value = SleepTimer / SleepTime;
            }
            else
            {
                SleepButton.interactable = true;
                SleepSlider.gameObject.SetActive(false);

            }
            return;

        }
        DungeonText.text = Level.ToString();


    }

    public void SetEnemyTypes()
    {
        var beasts = 0;
        var ghosts = 0;
        var demons = 0;
        var skellys = 0;
        for (var i = 0; i < SpawnIcons.Length; i++)
        {
            SpawnIcons[i].sprite = TypeSprites[SpawnTypes[i]];
            if(SpawnTypes[i] == 0 && i < 3)
            {
                beasts++;
            }
            if (SpawnTypes[i] == 1 && i < 3)
            {
                ghosts++;
            }
            if (SpawnTypes[i] == 2 && i < 3)
            {
                demons++;
            }
            if (SpawnTypes[i] == 3 && i < 3)
            {
                skellys++;
            }
        }

        Debug.Log("Beasts:" + beasts + " Ghosts:" + ghosts + " Demons:" + demons + " Skellys:" + skellys);
        Stock.CalculateWeaponPrices(beasts, ghosts, demons, skellys);
        Stock.CalculateArmourAndPotions(EnemyCount, EnemyStrength);

        

    }

    public void SpawnEnemies()
    {
        
        for(var i = 0; i < EnemyCount*2; i++)
        {
            if(i < 2)
            {
                EnemySpawners[i].SpawnEnemy(SpawnTypes[0], Level + EnemyStrength, SurvivalMode);
            }
            else if(i < 4)
            {
                EnemySpawners[i].SpawnEnemy(SpawnTypes[1], Level + EnemyStrength, SurvivalMode);
            }
            else
            {
                EnemySpawners[i].SpawnEnemy(SpawnTypes[2], Level + EnemyStrength, SurvivalMode);
            }
        }
    }

    public void NewLevel(int i)
    {
        var currentEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy x in currentEnemies)
        {
            Destroy(x.gameObject);
        }
        var allBones = GameObject.FindObjectsOfType<Bones>();
        foreach (Bones b in allBones)
        {
            Destroy(b.gameObject);
        }
        foreach (Prop p in Props)
        {
            p.SetSprite();
        }
        Level = i;
        var bonusText = GoldBonus.ToString("F1");
        GoldMultiplierText.text = "x" + bonusText;
        DungeonText.text = Level.ToString();
        
        
        GenerateLayout();
        SpawnEnemies();
        
    }

    public void CycleDungeon()
    {
        if (!Tutorial)
        {
            Save.SaveGame();
            var allHeroes = GameObject.FindObjectsOfType<Stats>();
            var hiredHeroes = new List<Stats>();
            foreach(Stats s in allHeroes)
            {
                if(s.State != Stats.HeroState.NotHired)
                {
                    hiredHeroes.Add(s);
                }
            }
            var allItems = GameObject.FindObjectsOfType<Item>();
            var boughtItems = new List<Item>();
            foreach(Item i in allItems)
            {
                if (!i.Merchant)
                {
                    boughtItems.Add(i);
                }
            }

            if(boughtItems.Count == 0 && hiredHeroes.Count == 0)
            {
                Debug.Log("Player has no items or heroes");
                if(Stock.Gold < 100)
                {
                    Stock.CollectGold(100);
                    Debug.Log("Player gifted 100G");
                }
            }
        }
        sfx.PlaySound(NewDaySound);
        EnemyCount = NextCount;
        if (EnemyCount > 1 && EnemyCount < 3)
        {
            NextCount = Mathf.Clamp(EnemyCount + Random.Range(-1, 2), 1, 3);
        }
        else
        {
            if (EnemyCount == 1)
            {
                NextCount = Mathf.Clamp(EnemyCount + Random.Range(0, 2), 1, 3);
            }
            else if (EnemyCount == 3)
            {
                NextCount = Mathf.Clamp(EnemyCount + Random.Range(-1, 1), 1, 3);
            }
        }
        EnemyCountSlider.value = EnemyCount * 0.333f + 0.04f;
        if (NextCount == EnemyCount)
        {
            CountTrendIcon.enabled = false;
        }
        else
        {
            CountTrendIcon.enabled = true;
            if (NextCount > EnemyCount)
            {
                CountTrendIcon.sprite = UpSprite;
            }
            else
            {
                CountTrendIcon.sprite = DownSprite;
            }
        }
        EnemyStrength = NextStrength;
        if (EnemyStrength > 1 && EnemyStrength < 3)
        {
            NextStrength = Mathf.Clamp(EnemyStrength + Random.Range(-1, 2), 1, 3);
        }
        else
        {
            if (EnemyStrength == 1)
            {
                NextStrength = Mathf.Clamp(EnemyStrength + Random.Range(0, 2), 1, 3);
            }
            else if (EnemyStrength == 3)
            {
                NextStrength = Mathf.Clamp(EnemyStrength + Random.Range(-1, 1), 1, 3);
            }
        }
        EnemyStrengthSlider.value = EnemyStrength * 0.333f + 0.04f;
        if (NextStrength == EnemyStrength)
        {
            StrengthTrendIcon.enabled = false;
        }
        else
        {
            StrengthTrendIcon.enabled = true;
            if (NextStrength > EnemyStrength)
            {
                StrengthTrendIcon.sprite = UpSprite;
            }
            else
            {
                StrengthTrendIcon.sprite = DownSprite;
            }
        }
        SpawnTypes[0] = SpawnTypes[1];
        SpawnTypes[1] = SpawnTypes[2];
        SpawnTypes[2] = SpawnTypes[3];
        SpawnTypes[3] = Random.Range(0, 4);
        SetEnemyTypes();
        Inn.NewDay();
        CurrentTime = 0;
        string hourDisplay = Hours.ToString("00");
        string minuteDisplay = Minutes.ToString("00");
        TimeText.text = hourDisplay + ":" + minuteDisplay;
        TimeSlider.value = CurrentTime / MaxTime;

    }

    public void StartDungeon(int i)
    {
        sfx.PlaySound(EnterDungeonSound);
        HeroSprite.enabled = true;
        var currentEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy x in currentEnemies)
        {
            Destroy(x.gameObject);
        }
        var allBones = GameObject.FindObjectsOfType<Bones>();
        foreach (Bones b in allBones)
        {
            Destroy(b.gameObject);
        }
        foreach (Prop p in Props)
        {
            p.SetSprite();
        }
        DungeonCompleted = false;
        GoldBonus = 1;
        GoldMultiplierText.text = GoldBonus + "x";
        HeroUI.SetActive(true);
        SleepButton.gameObject.SetActive(false);
        Running = true;
        CurrentHeroAI.Waiting = false;
        if (!SurvivalMode)
        {
            HeroSprite.sprite = CurrentHeroStats.HeroSprite;
            HeroImage.sprite = CurrentHeroStats.HeroSprite;
            if (CurrentHeroStats.ConsumableItem != null)
            {
                ConsumableIcon.sprite = CurrentHeroStats.ConsumableItem.ItemSprite.sprite;
                if(CurrentHeroStats.ConsumableItem.GetComponent<Consumable>().Type == Consumable.ConsumableType.Damage)
                {
                    ConsumableText.gameObject.SetActive(true);
                    ConsumableText.text = CurrentHeroStats.ConsumableItem.GetComponent<Consumable>().Value.ToString();
                }
                else if (CurrentHeroStats.ConsumableItem.GetComponent<Consumable>().Type == Consumable.ConsumableType.Potion)
                {
                    ConsumableText.gameObject.SetActive(true);
                    ConsumableText.text = CurrentHeroStats.ConsumableItem.GetComponent<Consumable>().Value.ToString();
                }
                else
                {
                    ConsumableText.gameObject.SetActive(false);
                }
            }
            else
            {
                ConsumableIcon.sprite = HandSprite;
                ConsumableText.gameObject.SetActive(false);
            }
        }
        
        CurrentHeroAI.Agent.Warp(HeroStartPosition);
        CurrentHeroAI.ResetCamera();
        Level = i;
        GenerateLayout();
        SetEnemyTypes();
        SpawnEnemies();
        
    }

    public void NextDungeonLevel()
    {
        readyForNextDungeon = true;
        timer = 0;
        

    }
    public void SendToNextLevel()
    {
        Level++;
        DungeonText.text = Level.ToString();
        
        sfx.PlaySound(EnterDungeonSound);
        var allBones = GameObject.FindObjectsOfType<Bones>();
        foreach (Bones b in allBones)
        {
            Destroy(b.gameObject);
        }
        foreach (Prop p in Props)
        {
            p.SetSprite();
        }
        if (Level > CurrentHeroAI.Manager.MaxDungeonFloor)
        {
            CurrentHeroAI.Manager.MaxDungeonFloor++;
            CurrentHeroAI.Manager.CheckFloorButtons();
            if(Level >= 20)
            {
                var ach = new Steamworks.Data.Achievement("dungeon_master");
                ach.Trigger();
            }
        }
        DungeonCompleted = false;
        GoldBonus += 0.4f;
        GoldBonus = Mathf.Clamp(GoldBonus, 1, 10);
        var bonusText = GoldBonus.ToString("F1");
        GoldMultiplierText.text = "x" + bonusText;
        HeroUI.SetActive(true);
        SleepButton.gameObject.SetActive(false);
        Running = true;
        CurrentHeroAI.Waiting = false;
        if (SurvivalMode)
        {
            DungeonText.text = Level.ToString();
            if (Level % 4 == 0)
            {
                SurvivalMerchantUpgrade();
            }
        }
        HeroImage.sprite = CurrentHeroStats.HeroSprite;
        CurrentHeroAI.Agent.Warp(HeroStartPosition);
        CurrentHeroAI.ResetCamera();
        GenerateLayout();
        SetEnemyTypes();
        SpawnEnemies();
    }

    public void DungeonComplete()
    {
        if (!Tutorial && !SurvivalMode)
        {
            Save.SaveGame();
        }
       
        TopContent.NewSaleIcon();
        DungeonCompleteMenu.Activate();
        if (DungeonCompleted)
        {
            sfx.PlaySound(DungeonCompleteSound);
            if (SurvivalMode)
            {
                levelCount++;
                if (levelCount > PlayerPrefs.GetFloat("HS"))
                {
                    newHighScore = true;
                    PlayerPrefs.SetFloat("HS", levelCount);
                }
            }
            

            CompleteTitle.text = "LEVEL COMPLETE!";
            var bonusText = GoldBonus + 0.4f;
            bonusText = Mathf.Clamp(bonusText, 1, 10);
            if (!SurvivalMode)
            {
                CompleteText.text = "Return home or continue for " + bonusText.ToString("F1") + "x gold discovery?";
            }
            else
            {
                CompleteText.text = "Levels Cleared: " + levelCount.ToString() + "\n High Score: " + PlayerPrefs.GetFloat("HS").ToString();
            }
            
            GoldCollectedText.text = CurrentHeroStats.GoldHeld.ToString();
            LootCollectedText.text = CurrentHeroStats.LootHeld.ToString();
            LevelText.text = Level.ToString();
            CompleteImage.sprite = CompleteSprite;
            CompleteButtons.SetActive(true);
            RIPButton.SetActive(false);
        }
        else
        {
            if (!SurvivalMode)
            {
                CompleteTitle.text = CurrentHeroStats.HeroName + " DIED!";
                CompleteText.text = "All gold and loot collected by the hero is lost!";
            }
            else
            {
                CompleteTitle.text = "YOU DIED!";
                CompleteText.text = "Levels Cleared: " + levelCount.ToString() + "\n High Score: " + PlayerPrefs.GetFloat("HS").ToString();
            }

            CompleteImage.sprite = DeadSprite;
            CompleteButtons.SetActive(false);
            RIPButton.SetActive(true);
        }
    }

    public void GenerateLayout()
    {
        for (var l = 0; l < DungeonLayouts.Count; l++)
        {
            DungeonLayouts[l].SetActive(false);
        }
        var layout = Random.Range(0, DungeonLayouts.Count);
        DungeonLayouts[layout].SetActive(true);
    }

    public void Sleep()
    {
        SleepTimer = 0;
        CycleDungeon();
    }

    public void EndTutorial()
    {
        Tutorial = false;
    }
    public void UnPause()
    {
        Paused = false;
    }

    public void UpdateUI()
    {
        EnemyStrengthSlider.value = EnemyStrength * 0.333f + 0.04f;
        if (NextStrength == EnemyStrength)
        {
            StrengthTrendIcon.enabled = false;
        }
        else
        {
            StrengthTrendIcon.enabled = true;
            if (NextStrength > EnemyStrength)
            {
                StrengthTrendIcon.sprite = UpSprite;
            }
            else
            {
                StrengthTrendIcon.sprite = DownSprite;
            }
        }
        EnemyCountSlider.value = EnemyCount * 0.333f + 0.04f;
        if (NextCount == EnemyCount)
        {
            CountTrendIcon.enabled = false;
        }
        else
        {
            CountTrendIcon.enabled = true;
            if (NextCount > EnemyCount)
            {
                CountTrendIcon.sprite = UpSprite;
            }
            else
            {
                CountTrendIcon.sprite = DownSprite;
            }
        }
    }

    public void StoreData()
    {
        var newDungeonData = new DungeonData();
        newDungeonData.Time = CurrentTime;
        newDungeonData.EnemyCount = EnemyCount;
        newDungeonData.NextCount = NextCount;
        newDungeonData.EnemyStrength = EnemyStrength;
        newDungeonData.NextStrength = NextStrength;
        var typeList = new List<int>();
        foreach(int i in SpawnTypes)
        {
            typeList.Add(i);
        }
        newDungeonData.SpawnTypes = typeList.ToArray();
        newDungeonData.SleepTimer = SleepTimer;

        SaveData = newDungeonData;

    }

    public void SurvivalPlayerUpgrade()
    {
        CurrentHeroAI.Waiting = true;
        Running = false;
        PlayerUpgradeMenu.GetComponent<PlayerUpgradeMenu>().NewUpgrades(false);
    }

    public void SurvivalMerchantUpgrade()
    {
        CurrentHeroAI.Waiting = true;
        Running = false;
        PlayerUpgradeMenu.GetComponent<PlayerUpgradeMenu>().NewUpgrades(true);
    }
}
