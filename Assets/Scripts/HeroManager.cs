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
    public Image HeroSprite;
    public DungeonManager DM;
    public int MaxDungeonFloor = 1;
    public int SelectedFloor = 1;
    public Button NextFloor;
    public Button PreviousFloor;
    public TextMeshProUGUI SelectedFloorText;
    public Menu QuestMenu;

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
            HeroInfoText.text = s.HeroName + "\n Level " + s.Level + "  " + s.Class + "\n HP:" + s.MaxHP + "\n DMG:" + s.Damage;
        }

        HeroNameText.text = s.HeroName;
        QuestText.text = "Level " + s.Level + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + "/" + s.MaxXP + "\n Damage:" + s.Damage + "\n Discovery: x" + s.Discovery;
        HeroSprite.sprite = s.HeroSprite;

    }

    public void StartDungeon()
    {
        DM.CurrentHeroStats = SelectedHero;
        DM.CurrentHeroAI.Stats = SelectedHero;
        DM.StartDungeon(SelectedFloor);
        SelectedHero.State = Stats.HeroState.Questing;
        HeroInfoText.text = SelectedHero.HeroName + " is on a Quest!";
        CurrentQuestingHero = SelectedHero.gameObject;
    }

    public void SendOnQuest()
    {
        if(CurrentQuestingHero != null)
        {
            HeroInfoText.text = "A Hero is already on a Quest!";
        }
        else
        {
            QuestMenu.Activate();
        }
    }

    public void CheckFloorButtons()
    {
        if (SelectedFloor == 1)
        {
            PreviousFloor.interactable = false;
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
        SelectedFloorText.text = SelectedFloor.ToString();
    }

    public void UpFloorLevel()
    {
        if(SelectedFloor < MaxDungeonFloor)
        {
            SelectedFloor++;
        }
        CheckFloorButtons();
    }
    public void DownFloorLevel()
    {
        if (SelectedFloor > 1)
        {
            SelectedFloor--;
        }
        CheckFloorButtons();
    }
    public void ReturnHero()
    {
        SelectedFloor = 1;
        CurrentQuestingHero = null;
        if(SelectedHero.State == Stats.HeroState.Questing)
        {
            SelectedHero.State = Stats.HeroState.Idle;
        }
    }
}
