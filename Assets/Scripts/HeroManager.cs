using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HeroManager : MonoBehaviour
{
    public Stats SelectedHero;
    public HeroAI DungeonHero;
    public GameObject CurrentQuestingHero;
    public TextMeshProUGUI HeroInfoText;
    public TextMeshProUGUI HeroNameText;
    public TextMeshProUGUI QuestText;
    public EquipMenu EquipScreen;
    public Image HeroSprite;
    public DungeonManager DM;
    public int MaxDungeonFloor = 1;
    public int SelectedFloor = 1;
    public Button NextFloor;
    public Button PreviousFloor;
    public Button GoButton;
    public TextMeshProUGUI SelectedFloorText;
    public TextMeshProUGUI DeployCostText;
    public GameObject QuestBlocker;
    public StockManager Stock;
    public Menu QuestMenu;
    public Menu EquipMenu;
    public Image EquipHeroImage;
    public int DeployCost = 0;
    public Stats[] AllHeroes;
    public int CurrentHero;
    public AudioClip ReturnSound;
    public GameObject InfoPanel;

    private SFXManager sfx;
    // Start is called before the first frame update
    void Start()
    {
        sfx = GameObject.FindObjectOfType<SFXManager>();
        CheckFloorButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectHero(Stats s)
    {
        SelectedHero = s;
        s.Selected = true;
        if(s.WeaponItem != null)
        {
            s.WeaponItem.gameObject.SetActive(true);
        }
        if (s.HelmItem != null)
        {
            s.HelmItem.gameObject.SetActive(true);
        }
        if (s.ArmourItem != null)
        {
            s.ArmourItem.gameObject.SetActive(true);
        }
        if (s.ConsumableItem != null)
        {
            s.ConsumableItem.gameObject.SetActive(true);
        }
        if (SelectedHero.State == Stats.HeroState.Dead)
        {
            HeroInfoText.text = s.HeroName + " IS DEAD!";
        }
        else if(SelectedHero.State == Stats.HeroState.Questing)
        {
            HeroInfoText.text = s.HeroName + " is on a Quest!";
        }
        else if(SelectedHero.State == Stats.HeroState.Training)
        {
            HeroInfoText.text = s.HeroName + " is currently training.";
        }
        else if(SelectedHero.State == Stats.HeroState.Idle)
        {
            InfoPanel.SetActive(true);

            HeroInfoText.text = s.HeroName + "\n"+ s.Class + "\n HP:" + s.MaxHP + "\n DMG:" + s.Damage + "\n RNG:" + Mathf.FloorToInt(s.Range) + "\n GOLD:x" + s.Discovery;
        }

        UpdateQuestMenu(SelectedHero);
        var heroes = GameObject.FindObjectsOfType<Stats>();
        for(var i = 0; i < heroes.Length; i ++)
        {
            if(heroes[i] != s)
            {
                heroes[i].Selected = false;
                if(heroes[i].WeaponItem != null)
                {
                    heroes[i].WeaponItem.gameObject.SetActive(false);
                }
                if (heroes[i].HelmItem != null)
                {
                    heroes[i].HelmItem.gameObject.SetActive(false);
                }
                if (heroes[i].ArmourItem != null)
                {
                    heroes[i].ArmourItem.gameObject.SetActive(false);
                }
                if (heroes[i].ConsumableItem != null)
                {
                    heroes[i].ConsumableItem.gameObject.SetActive(false);
                }
            }
            
            
        }
        
    }

    public void StartDungeon()
    {
        Stock.CollectGold(-DeployCost);
        DM.CurrentHeroStats = SelectedHero;
        DM.CurrentHeroAI.Stats = SelectedHero;
        DM.StartDungeon(SelectedFloor);
        SelectedHero.State = Stats.HeroState.Questing;
        HeroInfoText.text = SelectedHero.HeroName + " is on a Quest!";
        CurrentQuestingHero = SelectedHero.gameObject;
        SelectedHero.SelectHero();
        CheckFloorButtons();
        Stock.DeselectItems();
        DeselectHero();
    }

    public void SendOnQuest()
    {
        if(SelectedHero == null)
        {
            HeroInfoText.text = "No hero selected!";
            return;
        }
        if(CurrentQuestingHero != null)
        {
            HeroInfoText.text = "A Hero is already on a Quest!";
        }
        else
        {
            if(SelectedHero.State == Stats.HeroState.Idle)
            {
                QuestMenu.Activate();                
            }

        }
        

    }

    public void CheckFloorButtons()
    {
        if (SelectedFloor == 1)
        {
            PreviousFloor.interactable = false;
            DeployCost = 0;
        }
        else
        {
            PreviousFloor.interactable = true;
        }
        if (SelectedFloor < MaxDungeonFloor)
        {
            NextFloor.interactable = true;
        }
        else
        {
            NextFloor.interactable = false;
        }
        if(Stock.Gold >= DeployCost)
        {
            GoButton.interactable = true;
        }
        else
        {
            GoButton.interactable = false;
        }
        DeployCostText.text = DeployCost + "G";
        SelectedFloorText.text = SelectedFloor.ToString();
    }

    public void UpFloorLevel()
    {
        if(SelectedFloor < MaxDungeonFloor)
        {
            SelectedFloor++;
            DeployCost += 10;
        }
        CheckFloorButtons();
    }
    public void DownFloorLevel()
    {
        if (SelectedFloor > 1)
        {
            SelectedFloor--;
            DeployCost -= 10;
        }
        CheckFloorButtons();
    }
    public void ReturnHero()
    {
        sfx.PlaySound(ReturnSound);
        var s = CurrentQuestingHero.GetComponent<Stats>();
        Stock.CollectGold(s.GoldHeld);
        for(var c = 0; c<s.ChestLevels.Count; c++)
        {
            Stock.AddChest(s.DamageType, s.ChestLevels[c]);
        }
        s.ChestLevels.Clear();
        s.GoldHeld = 0;
        s.LootHeld = 0;
        s.HP = s.MaxHP;
        s.State = Stats.HeroState.Idle;
        SelectHero(s);
        s.SelectHero();
        CurrentQuestingHero = null;        
        DM.Level = 1;
        DM.GoldBonus = 1;
        DM.CurrentHeroAI.Agent.Warp(DM.HeroStartPosition);
        DM.CurrentHeroStats = null;
        DM.NewLevel(1);
        

    }
    public void RIPHero()
    {
        var s = CurrentQuestingHero.GetComponent<Stats>();
        SelectHero(s);
        s.SelectHero();
        s.GoldHeld = 0;
        s.State = Stats.HeroState.Dead;
        Destroy(s.gameObject);
        CurrentQuestingHero = null;
        DM.Level = 1;
        DM.GoldBonus = 1;
        DM.CurrentHeroAI.Agent.Warp(DM.HeroStartPosition);
        DM.CurrentHeroStats = null;
        DM.NewLevel(1);
        InfoPanel.SetActive(false);


    }
    public void DeleteHero()
    {
        Destroy(SelectedHero.gameObject);
        SelectedHero = null;
        InfoPanel.SetActive(false);

    }
    public void OpenEquipMenu()
    {
        if(SelectedHero == null)
        {
            HeroInfoText.text = "SELECT A HERO";
            InfoPanel.SetActive(false);
            return;
        }
        if(SelectedHero.State != Stats.HeroState.Idle)
        {
            HeroInfoText.text = "Cannot Equip this Hero";
            return;
        }
        EquipMenu.Activate();
        EquipScreen.UpdateEquipMenu(SelectedHero);
        
    }

    public void UpdateQuestMenu(Stats s)
    {
        AllHeroes = GameObject.FindObjectsOfType<Stats>().Where( h => h.State != Stats.HeroState.NotHired).ToArray();
        for (var i = 0; i < AllHeroes.Length; i++)
        {
            if (AllHeroes[i] == s)
            {
                CurrentHero = i;
            }
            
            
        }
        if(AllHeroes[CurrentHero].State == Stats.HeroState.Questing)
        {
            QuestBlocker.SetActive(true);
        }
        else
        {
            QuestBlocker.SetActive(false);
        }
        HeroNameText.text = s.HeroName;
        QuestText.text = "Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + "/" + s.MaxXP + "\n Damage:" + s.Damage + "\n Range:" + Mathf.FloorToInt(s.Range) + "\n Gold Drop:x" + s.Discovery;
        HeroSprite.sprite = s.HeroSprite;
    }

    public void NextHero()
    {
        if(AllHeroes.Length == 0)
        {
            CurrentHero = 0;
        }
        else
        {
            if (CurrentHero < AllHeroes.Length - 1)
            {
                CurrentHero++;
            }
            else
            {
                CurrentHero = 0;
            }
        }
        
        SelectHero(AllHeroes[CurrentHero]);
        AllHeroes[CurrentHero].SelectHero();
        UpdateQuestMenu(AllHeroes[CurrentHero]);

    }
    public void PreviousHero()
    {
        if (AllHeroes.Length == 0)
        {
            CurrentHero = 0;
        }
        else
        {
            if (CurrentHero > 0)
            {
                CurrentHero--;
            }
            else
            {
                CurrentHero = AllHeroes.Length - 1;
            }
        }
        SelectHero(AllHeroes[CurrentHero]);
        AllHeroes[CurrentHero].SelectHero();
        UpdateQuestMenu(AllHeroes[CurrentHero]);

    }

    public void DeselectHero()
    {
        SelectedHero = null;
        InfoPanel.SetActive(false);

    }

    public void SelectTutorialHero()
    {
        var hero = GameObject.FindObjectOfType<Stats>();
        hero.SelectHero();
    }


}
