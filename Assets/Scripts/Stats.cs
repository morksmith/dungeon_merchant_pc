using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Steamworks.Data;

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
    public Slider TrainSlider;
    public TextMeshProUGUI LevelText;
    public GameObject DeadText;
    public GameObject QuestText;
    public GameObject LevelUI;
    public GameObject TrainUI;
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
    public float TrainingTime = 300;
    public float TrainCost;
    public float Timer;
    public float XPBonus = 1;
    public List<int> ChestLevels;
    public HeroData Data;
    

    private void Start()
    {
        Manager = GameObject.FindObjectOfType<HeroManager>();
        LevelText.text = Level.ToString();
        TrainCost = 20 * Level;
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

        if(State == HeroState.Training)
        {
            TrainSlider.value = Timer / TrainingTime;
            if(Timer < TrainingTime)
            {
                Timer += Time.deltaTime;
            }
            else
            {
                FinishTraining();
            }
            

        }
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
        if(Level >= 10)
        {
            var ach = new Steamworks.Data.Achievement("legendary_hero");
            ach.Trigger();
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
            TrainCost = 20 * Level;
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
        if (WeaponItem.Special)
        {
            if(WeaponItem.WeaponBonus == Item.WeaponBonusType.Range)
            {
                Range += WeaponItem.RangeBonus;
            }
            else if(WeaponItem.WeaponBonus == Item.WeaponBonusType.Gold)
            {
                Discovery *= WeaponItem.GoldBonus;
            }
            else if (WeaponItem.WeaponBonus == Item.WeaponBonusType.HP)
            {
                MaxHP += WeaponItem.HPBonus;
                HP = MaxHP;
            }
            else if (WeaponItem.WeaponBonus == Item.WeaponBonusType.XP)
            {
                XPBonus = WeaponItem.XPBonus;
            }
        }
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
        if (WeaponItem.Special)
        {
            if (WeaponItem.WeaponBonus == Item.WeaponBonusType.Range)
            {
                Range -= WeaponItem.RangeBonus;
            }
            else if (WeaponItem.WeaponBonus == Item.WeaponBonusType.Gold)
            {
                Discovery /= WeaponItem.GoldBonus;
            }
            else if (WeaponItem.WeaponBonus == Item.WeaponBonusType.HP)
            {
                MaxHP -= WeaponItem.HPBonus;
                HP = MaxHP;
            }
            else if (WeaponItem.WeaponBonus == Item.WeaponBonusType.XP)
            {
                XPBonus = 1;
            }
        }
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
    public void FireHero()
    {
        if (WeaponItem != null)
        {
            WeaponItem.ReturnToStock();
            UnequipWeapon(WeaponItem.GetComponent<Weapon>());
            
        }
        if (ArmourItem != null)
        {
            ArmourItem.ReturnToStock();
            UneQuipArmour(ArmourItem.GetComponent<Armour>());
            

        }
        if (HelmItem != null)
        {
            HelmItem.ReturnToStock();
            UnequipHelm(HelmItem.GetComponent<Armour>());
            

        }
        if (ConsumableItem != null)
        {
            ConsumableItem.ReturnToStock();
            UnequipConsumable(ConsumableItem.GetComponent<Consumable>());
            
            

        }
        StoreData();

    }

    public void TrainHero()
    {
        State = HeroState.Training;
        Timer = 0;
        TrainUI.SetActive(true);
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
        if(State == HeroState.Training)
        {
            newHeroData.Training = true;
        }
        newHeroData.Timer = Timer;
        Data = newHeroData;

    }

    public void CheckTimePassed()
    {
        var lastDateChecked = System.DateTime.Parse(PlayerPrefs.GetString("DateTime"));
        var elapsedSeconds = (int)System.DateTime.Now.Subtract(lastDateChecked).TotalSeconds;
        Timer += elapsedSeconds;
        if(Timer > TrainingTime)
        {
            FinishTraining();
        }
    }

    public void FinishTraining()
    {
        Level++;
        MaxXP *= 1.5f;
        MaxXP = Mathf.CeilToInt(MaxXP);
        MaxHP += 10;
        Damage++;
        HP = MaxHP;
        LevelText.text = Level.ToString();
        State = HeroState.Idle;
        Timer = 0;
        TrainUI.SetActive(false);
        GameObject.FindObjectOfType<DungeonManager>().TopContent.NewItemIcon();
        GameObject.FindObjectOfType<StockManager>().PlayLevelUpSound();
        TrainCost = 20 * Level;
        StoreData();

    }
   

}
