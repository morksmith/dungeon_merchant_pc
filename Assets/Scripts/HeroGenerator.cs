using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGenerator : MonoBehaviour
{
    
    public List<string> KnightFirstNames;
    public List<string> KnightLastNames;
    public List<string> MageFirstNames;
    public List<string> MageLastNames;
    public List<string> RogueFirstNames;
    public List<string> RogueLastNames;
    public List<string> WarriorFirstNames;
    public List<string> WarriorLastNames;
    public List<Sprite> SpriteIndex;
    public ItemGenerator ItemGen;
    public EquipMenu Equipment; 
    public List<GameObject> HeroPrefabs;
    public GameObject TutorialHero;
    public Transform HeroParent;
    public HireMenu HireScreen;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    CreateHero();
        //}
    }
    private void Start()
    {

    }
    public void CreateHero()
    {
        var pick = Random.Range(0, HeroPrefabs.Count);
        var newHero = Instantiate(HeroPrefabs[pick], HeroParent.position, HeroParent.rotation, HeroParent);
        if(pick == 0)
        {
            var fn = Random.Range(0, KnightFirstNames.Count);
            var ln = Random.Range(0, KnightLastNames.Count);
            newHero.name = KnightFirstNames[fn] + KnightLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;
        }
        if(pick == 1)
        {
            var fn = Random.Range(0, MageFirstNames.Count);
            var ln = Random.Range(0, MageLastNames.Count);
            newHero.name = MageFirstNames[fn] + MageLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;

        }
        if (pick == 2)
        {
            var fn = Random.Range(0, RogueFirstNames.Count);
            var ln = Random.Range(0, RogueLastNames.Count);
            newHero.name = RogueFirstNames[fn] + RogueLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;

        }
        if (pick == 3)
        {
            var fn = Random.Range(0, WarriorFirstNames.Count);
            var ln = Random.Range(0, WarriorLastNames.Count);
            newHero.name = WarriorFirstNames[fn] + WarriorLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;

        }

        newHero.GetComponent<Stats>().Level = Random.Range(1, 3);
        if(newHero.GetComponent<Stats>().Level == 2)
        {
            newHero.GetComponent<Stats>().MaxXP *= 1.5f;
            newHero.GetComponent<Stats>().MaxXP = Mathf.CeilToInt(newHero.GetComponent<Stats>().MaxXP);
            newHero.GetComponent<Stats>().MaxHP += 10;
            newHero.GetComponent<Stats>().Damage++;
            newHero.GetComponent<Stats>().HP = newHero.GetComponent<Stats>().MaxHP;
        }
        HireScreen.UpdateHeroInfo(newHero.gameObject.transform);
        newHero.gameObject.SetActive(false);
        HireScreen.HiredHero = newHero.GetComponent<Stats>().Data;

    }

    public void CreateHiredHero(HeroData hd)
    {
        var newHero = Instantiate(HeroPrefabs[hd.DamageType]);
        var newHeroStats = newHero.GetComponent<Stats>();
        newHeroStats.HeroSprite = SpriteIndex[hd.SpriteIndex];
        newHeroStats.gameObject.name = hd.HeroName;
        newHeroStats.HeroName = hd.HeroName;
        newHeroStats.Level = hd.Level;
        newHeroStats.MaxHP = hd.MaxHP;
        newHeroStats.HP = hd.MaxHP;
        newHeroStats.MaxXP = hd.MaxXP;
        newHeroStats.XP = hd.XP;
        newHeroStats.Damage = hd.Damage;
        newHeroStats.Range = hd.Range;
        newHeroStats.Discovery = hd.Discovery;
        newHeroStats.LootFind = hd.LootFind;
        HireScreen.UpdateHeroInfo(newHero.gameObject.transform);
        newHero.gameObject.SetActive(false);
        HireScreen.HiredHero = newHero.GetComponent<Stats>().Data;
        newHero.transform.localScale = Vector3.one;
    }
    public void CreateTutorialHero()
    {
        var newHero = Instantiate(TutorialHero, HeroParent.position, HeroParent.rotation, HeroParent);
        newHero.GetComponent<Stats>().Level = 1;
        HireScreen.UpdateHeroInfo(newHero.gameObject.transform);
        newHero.gameObject.SetActive(false);
    }

    public void CreateSpecificHero(HeroData hd)
    {
        var newHero = Instantiate(HeroPrefabs[hd.DamageType]);
        var newHeroStats = newHero.GetComponent<Stats>();
        newHeroStats.HeroSprite = SpriteIndex[hd.SpriteIndex];
        newHeroStats.gameObject.name = hd.HeroName;
        newHeroStats.HeroName = hd.HeroName;
        newHeroStats.Level = hd.Level;
        newHeroStats.MaxHP = hd.MaxHP;
        newHeroStats.HP = hd.MaxHP;
        newHeroStats.MaxXP = hd.MaxXP;
        newHeroStats.XP = hd.XP;
        newHeroStats.Damage = hd.Damage;
        newHeroStats.Range = hd.Range;
        newHeroStats.Discovery = hd.Discovery;
        newHeroStats.LootFind = hd.LootFind;
        newHeroStats.State = Stats.HeroState.Idle;
        if (hd.hasWeapon)
        {
            ItemGen.CreateEquippedItem(hd.WeaponData, Equipment.WeaponItemSlot, newHeroStats);
        }
        else
        {
            newHeroStats.WeaponItem = null;
        }
        if (hd.hasArmour)
        {
            ItemGen.CreateEquippedItem(hd.ArmourData, Equipment.ArmourSlot, newHeroStats);
        }
        else
        {
            newHeroStats.ArmourItem = null;
        }
        if (hd.hasHelm)
        {
            ItemGen.CreateEquippedItem(hd.HelmData, Equipment.HelmSlot, newHeroStats);
        }
        else
        {
            newHeroStats.HelmItem = null;
        }
        if (hd.hasConsumable)
        {
            ItemGen.CreateEquippedItem(hd.ConsumableData, Equipment.ConsumableSlot, newHeroStats);
        }
        else
        {
            newHeroStats.ConsumableItem = null;
        }
        newHero.transform.SetParent(GameObject.FindObjectOfType<HireMenu>().HeroParent);
        newHero.transform.localScale = Vector3.one;
        

    }
}
