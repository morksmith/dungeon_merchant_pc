using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public HeroClass Class;
    public Sprite HeroSprite;
    public float Level;
    public float MaxHP;
    public float HP;
    public float MaxXP;
    public float XP;
    public float Damage;
    public float Range;
    public float Accuracy = 80;
    public float Discovery = 1;
    public float SkillPoints = 0;
    public float GoldHeld;
    public float LootHeld;

    // Update is called once per frame
    void Update()
    {
        if(XP >= MaxXP)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level++;
        XP = XP - MaxXP;
        MaxXP *= 1.5f;
        MaxXP = Mathf.CeilToInt(MaxXP);
        SkillPoints += 1;
        MaxHP += 10;
        Damage++;
        HP = MaxHP;
    }
}
