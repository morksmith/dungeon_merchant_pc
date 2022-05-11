using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroManager : MonoBehaviour
{
    public Stats SelectedHero;
    public HeroAI DungeonHero;
    public GameObject CurrentQuestingHero;
    public TextMeshProUGUI HeroInfoText;
    public TextMeshProUGUI HeroNameText;
    public TextMeshProUGUI QuestText;
    public TextMeshProUGUI EquipText;
    public Image HeroSprite;
    public DungeonManager DM;
    public int MaxDungeonFloor = 1;
    public int SelectedFloor = 1;
    public Button NextFloor;
    public Button PreviousFloor;
    public Button GoButton;
    public TextMeshProUGUI SelectedFloorText;
    public TextMeshProUGUI DeployCostText;
    public StockManager Stock;
    public Menu QuestMenu;
    public Menu EquipMenu;
    public Image EquipHeroImage;
    public int DeployCost = 0;

    // Start is called before the first frame update
    void Start()
    {
        CheckFloorButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectHero(Stats s)
    {
        SelectedHero = s;
        if(SelectedHero.State == Stats.HeroState.Dead)
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
            HeroInfoText.text = s.HeroName + "\n"+ s.Class + "\n HP:" + s.MaxHP + "\n DMG:" + s.Damage + "\n RNG:" + Mathf.FloorToInt(s.Range) + "\n GOLD:x" + s.Discovery;
        }

        HeroNameText.text = s.HeroName;
        QuestText.text = "Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + "/" + s.MaxXP + "\n Damage:" + s.Damage + "\n Range:" + Mathf.FloorToInt(s.Range) + "\n Gold Drop:x" + s.Discovery;
        HeroSprite.sprite = s.HeroSprite;

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
        SelectedFloor = 1;
        CheckFloorButtons();
    }

    public void SendOnQuest()
    {
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
            DeployCost += 50;
        }
        CheckFloorButtons();
    }
    public void DownFloorLevel()
    {
        if (SelectedFloor > 1)
        {
            SelectedFloor--;
            DeployCost -= 50;
        }
        CheckFloorButtons();
    }
    public void ReturnHero()
    {
        
        Stock.CollectGold(SelectedHero.GoldHeld);
        SelectedHero.GoldHeld = 0;
        SelectedHero.LootHeld = 0;
        SelectedHero.HP = SelectedHero.MaxHP;
        CurrentQuestingHero = null;
        SelectedHero.State = Stats.HeroState.Idle;
        DM.Level = 1;
        DM.GoldBonus = 1;
        DM.CurrentHeroAI.Agent.Warp(DM.HeroStartPosition);
        DM.CurrentHeroStats = null;
        DM.NewLevel(1);
        SelectHero(SelectedHero);
        SelectedHero.SelectHero();

    }
    public void RIPHero()
    {
        SelectedFloor = 1;
        CurrentQuestingHero = null;
        SelectedHero.GoldHeld = 0;
        SelectedHero.State = Stats.HeroState.Dead;
        DM.Level = 1;
        DM.GoldBonus = 1;
        DM.CurrentHeroAI.Agent.Warp(DM.HeroStartPosition);
        DM.CurrentHeroStats = null;
        DM.NewLevel(1);
        SelectHero(SelectedHero);
        SelectedHero.SelectHero();
    }
    public void DeleteHero()
    {
        Destroy(SelectedHero.gameObject);
        SelectedHero = null;
    }
    public void OpenEquipMenu()
    {
        if(SelectedHero.State != Stats.HeroState.Idle)
        {
            HeroInfoText.text = "Cannot Equip this Hero";
            return;
        }
        var s = SelectedHero;
        EquipMenu.Activate();
        EquipText.text = s.HeroName + "\n Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + "/" + s.MaxXP + "\n Damage:" + s.Damage + "\n Range:" + Mathf.FloorToInt(s.Range) + "\n Gold Drop:x" + s.Discovery;
        EquipHeroImage.sprite = s.HeroSprite;
    }
}
