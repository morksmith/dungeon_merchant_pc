using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Stats : MonoBehaviour
{
    public string HeroName;
    public enum HeroClass
    {
        Warrior,
        Knight,
        Mage,
        Rogue
    }
    public enum HeroState
    {
        Idle,
        Questing,
        Training,
        Dead
    }
    public HeroState State;
    public HeroClass Class;
    public int DamageType;
    public HeroManager Manager;
    public Sprite HeroSprite;
    public Slider XPSlider;
    public TextMeshProUGUI LevelText;
    public GameObject DeadText;
    public GameObject QuestText;
    public GameObject LevelUI;
    public float Level;
    public float MaxHP;
    public float HP;
    public float MaxXP;
    public float XP;
    public float Damage;
    public float Range;
    public float Discovery = 1;
    public float SkillPoints = 0;
    public float GoldHeld;
    public float LootHeld;

    private void Start()
    {
        Manager = GameObject.FindObjectOfType<HeroManager>();
        LevelText.text = Level.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if(XP >= MaxXP)
        {
            LevelUp();
        }
        XPSlider.value = XP / MaxXP;
    }

    public void LevelUp()
    {

        Level++;
        XP = XP - MaxXP;
        MaxXP *= 1.8f;
        MaxXP = Mathf.CeilToInt(MaxXP);
        SkillPoints += 1;
        MaxHP += 10;
        Damage++;
        HP = MaxHP;
        LevelText.text = Level.ToString();
    }

    public void SelectHero()
    {
        Manager.SelectHero(this);
        if(State == HeroState.Dead)
        {
            DeadText.SetActive(true);
            QuestText.SetActive(false);
            LevelUI.SetActive(false);
        }
        else if(State == HeroState.Questing)
        {
            DeadText.SetActive(false);
            QuestText.SetActive(true);
            LevelUI.SetActive(false);
        }
        else if (State == HeroState.Idle)
        {
            DeadText.SetActive(false);
            QuestText.SetActive(false);
            LevelUI.SetActive(true);
        }
    }

    public void Die()
    {
        HP = 0;
        XP = 0;
        
        State = HeroState.Dead;
    }

   

}
