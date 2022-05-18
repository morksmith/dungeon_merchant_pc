using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonManager : MonoBehaviour
{
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
    public SpriteRenderer HeroSprite;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI DungeonText;
    public TextMeshProUGUI HeroLevel;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI LootText;
    public Stats CurrentHeroStats;
    public HeroAI CurrentHeroAI;
    public Slider HeroHP;
    public Slider HeroXP;
    public EnemySpawner[] EnemySpawners;
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
    // Start is called before the first frame update
    void Start()
    {
        
       
        EnemySpawners = GameObject.FindObjectsOfType<EnemySpawner>();
        EnemyCount = 1;
        NextCount = Mathf.Clamp(EnemyCount + Random.Range(-1, 2), 1, 3);
        EnemyCountSlider.value = EnemyCount * 0.333f + 0.04f;
        if(NextCount == EnemyCount)
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
        EnemyStrength = 1;
        NextStrength = Mathf.Clamp(EnemyCount + Random.Range(-1, 2), 1, 3);
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
        foreach(int i in SpawnTypes)
        {
            SpawnTypes[i] = Random.Range(0, 4);
        }
        SetEnemyTypes();



        Stock.UpdatePrices();


    }

    // Update is called once per frame
    void Update()
    {
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
            CurrentHeroAI.gameObject.SetActive(false);
        }

        if (!Running)
        {
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

        

    }

    public void SpawnEnemies()
    {
        
        for(var i = 0; i < EnemyCount*2; i++)
        {
            if(i < 2)
            {
                EnemySpawners[i].SpawnEnemy(SpawnTypes[0], Level + EnemyStrength);
            }
            else if(i < 4)
            {
                EnemySpawners[i].SpawnEnemy(SpawnTypes[1], Level + EnemyStrength);
            }
            else
            {
                EnemySpawners[i].SpawnEnemy(SpawnTypes[2], Level + EnemyStrength);
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
        Level = i;
        var bonusText = GoldBonus.ToString("F1");
        GoldMultiplierText.text = "x" + bonusText;
        DungeonText.text = Level.ToString();
        
        
        GenerateLayout();
        SpawnEnemies();
    }

    public void CycleDungeon()
    {
        EnemyCount = NextCount;
        NextCount = Mathf.Clamp(EnemyCount + Random.Range(-1, 2), 1, 3);
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
        NextStrength = Mathf.Clamp(EnemyCount + Random.Range(-1, 2), 1, 3);
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
        CurrentTime = 0;

    }

    public void StartDungeon(int i)
    {
        var currentEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy x in currentEnemies)
        {
            Destroy(x.gameObject);
        }
        DungeonCompleted = false;
        GoldBonus = 1;
        GoldMultiplierText.text = GoldBonus + "x";
        HeroUI.SetActive(true);
        Running = true;
        CurrentHeroAI.Waiting = false;        
        HeroImage.sprite = CurrentHeroStats.HeroSprite;
        HeroSprite.sprite = CurrentHeroStats.HeroSprite;
        CurrentHeroAI.Agent.Warp(HeroStartPosition);
        Level = i;
        GenerateLayout();
        SetEnemyTypes();
        SpawnEnemies();
        
    }

    public void NextDungeonLevel()
    {
        Level++;
        if(Level > CurrentHeroAI.Manager.MaxDungeonFloor)
        {
            CurrentHeroAI.Manager.MaxDungeonFloor++;
            CurrentHeroAI.Manager.CheckFloorButtons();
        }
        DungeonCompleted = false;
        GoldBonus *= 1.5f;
        GoldBonus = Mathf.Clamp(GoldBonus, 1, 10);
        var bonusText = GoldBonus.ToString("F1");
        GoldMultiplierText.text = "x" + bonusText;
        HeroUI.SetActive(true);
        Running = true;
        CurrentHeroAI.Waiting = false;
        HeroImage.sprite = CurrentHeroStats.HeroSprite;
        CurrentHeroAI.Agent.Warp(HeroStartPosition);
        GenerateLayout();
        SetEnemyTypes();
        SpawnEnemies();
    }

    public void DungeonComplete()
    {
        DungeonCompleteMenu.Activate();
        if (DungeonCompleted)
        {
            CompleteTitle.text = "LEVEL COMPLETE!";
            CompleteText.text = "Return home or continue for 1.5x gold discovery?";
            GoldCollectedText.text = CurrentHeroStats.GoldHeld.ToString();
            LootCollectedText.text = CurrentHeroStats.LootHeld.ToString();
            CompleteImage.sprite = CompleteSprite;
            CompleteButtons.SetActive(true);
            RIPButton.SetActive(false);
        }
        else
        {
            CompleteTitle.text = CurrentHeroStats.HeroName + " DIED!";
            CompleteText.text = "All gold and loot collected by the hero is lost!";
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
}
