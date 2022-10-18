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
        Ranger
    }
    public enum HeroState
    {
        NotHired,
        Idle,
        Questing,
        Training,
        Dead
    }
    public HeroState State;
    public HeroClass Class;
    public Item WeaponItem;
    public Item HelmItem;
    public Item ArmourItem;
    public Item ConsumableItem;
    public int DamageType;
    public HeroManager Manager;
    public Sprite HeroSprite;
    public int SpriteIndex;
    public Slider XPSlider;
    public TextMeshProUGUI LevelText;
    public GameObject DeadText;
    public GameObject QuestText;
    public GameObject LevelUI;
    public bool Selected;
    public float Level;
    public float MaxHP;
    public float HP;
    public float MaxXP;
    public float XP;
    public float Damage;
    public float Range;
    public float Discovery = 1;
    public float LootFind = 1;
    public float GoldHeld;
    public float LootHeld;
    public List<int> ChestLevels;
    public HeroData Data;
    

    private void Start()
    {
        Manager = GameObject.FindObjectOfType<HeroManager>();
        LevelText.text = Level.ToString();

        StoreData();
       
    }
    // Update is called once per frame
    void Update()
    {
        if (XP >= MaxXP)
        {
            LevelUp();
        }
        XPSlider.value = XP / MaxXP;
    }

    public void LevelUp()
    {
        var AI = GameObject.FindObjectOfType<HeroAI>();
        if (!AI.PlayerControlled)
        {
            Level++;
            XP = XP - MaxXP;
            MaxXP *= 1.5f;
            MaxXP = Mathf.CeilToInt(MaxXP);
            MaxHP += 10;
            Damage++;
            HP = MaxHP;
            LevelText.text = Level.ToString();
        }
        else
        {
            AI.DM.SurvivalPlayerUpgrade();
            Level++;
            XP = XP - MaxXP;
            MaxXP *= 1.5f;
            MaxXP = Mathf.CeilToInt(MaxXP);
            LevelText.text = Level.ToString();
        }
        
    }

    public void SelectHero()
    {
        if(State == HeroState.NotHired)
        {
            return;
        }
        Manager.SelectHero(this);
        if (State == HeroState.Dead)
        {
            DeadText.SetActive(true);
            QuestText.SetActive(false);
            LevelUI.SetActive(false);
        }
        else if (State == HeroState.Questing)
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
        StoreData();
    }

    public void Die()
    {
        HP = 0;
        XP = 0;
        RemoveItems();
        State = HeroState.Dead;

    }

    public void EquipWeapon(Weapon w)
    {
        Damage += w.Damage;
        WeaponItem = w.gameObject.GetComponent<Item>();
        StoreData();
    }

    public void EquipArmour(Armour a)
    {
        MaxHP += a.HP;
        HP = MaxHP;
        ArmourItem = a.gameObject.GetComponent<Item>();
        StoreData();

    }

    public void UneQuipArmour(Armour a)
    {
        MaxHP -= a.HP;
        HP = MaxHP;
        ArmourItem = null;
        StoreData();

    }
    public void EquipHelm(Armour a)
    {
        MaxHP += a.HP;
        HP = MaxHP;
        HelmItem = a.gameObject.GetComponent<Item>();
        StoreData();

    }

    public void UnequipHelm(Armour a)
    {
        MaxHP -= a.HP;
        HP = MaxHP;
        HelmItem = null;
        StoreData();

    }

    public void UnequipWeapon(Weapon w)
    {
        Damage -= w.Damage;
        WeaponItem = null;
        StoreData();

    }

    public void EquipConsumable(Consumable c)
    {
        ConsumableItem = c.gameObject.GetComponent<Item>();
        StoreData();

    }
    public void UnequipConsumable(Consumable c)
    {
        ConsumableItem = null;
        StoreData();

    }

    public void RemoveItems()
    {
        if(WeaponItem != null)
        {
            Destroy(WeaponItem.gameObject);
        }
        if (ArmourItem != null)
        {
            Destroy(ArmourItem.gameObject);
        }
        if (HelmItem != null)
        {
            Destroy(HelmItem.gameObject);
        }
        if (ConsumableItem != null)
        {
            Destroy(ConsumableItem.gameObject);
        }
        StoreData();

    }

    public void StoreData()
    {
        var newHeroData = new HeroData();
        if(WeaponItem != null)
        {
            newHeroData.WeaponData = WeaponItem.Data;
            newHeroData.hasWeapon = true;
        }
        else
        {
            newHeroData.WeaponData = null;
            newHeroData.hasWeapon = false;

        }
        if (ArmourItem != null)
        {
            newHeroData.ArmourData = ArmourItem.Data;
            newHeroData.hasArmour = true;
        }
        else
        {
            newHeroData.ArmourData = null;
            newHeroData.hasArmour = false;
        }
        if (HelmItem != null)
        {
            newHeroData.HelmData = HelmItem.Data;
            newHeroData.hasHelm = true;
        }
        else
        {
            newHeroData.HelmData = null;
            newHeroData.hasHelm = false;
        }
        if (ConsumableItem != null)
        {
            newHeroData.ConsumableData = ConsumableItem.Data;
            newHeroData.hasConsumable = true;
        }
        else
        {
            newHeroData.ConsumableData = null;
            newHeroData.hasConsumable = false;
        }
        newHeroData.DamageType = DamageType;
        newHeroData.HeroName = HeroName;
        newHeroData.SpriteIndex = SpriteIndex;
        newHeroData.Level = Level;
        newHeroData.MaxHP = MaxHP;
        newHeroData.MaxXP = MaxXP;
        newHeroData.XP = XP;
        newHeroData.Damage = Damage;
        newHeroData.Range = Range;
        newHeroData.Discovery = Discovery;
        newHeroData.LootFind = LootFind;
        if(State != HeroState.Dead && State != HeroState.NotHired)
        {
            newHeroData.Hired = true;
        }
        Data = newHeroData;

    }
   

}
