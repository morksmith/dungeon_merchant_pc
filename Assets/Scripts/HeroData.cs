using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class HeroData
{
    public string HeroName;
    public bool Hired;
    public bool hasWeapon = false;
    public bool hasHelm = false;
    public bool hasArmour = false;
    public bool hasConsumable = false;
    public ItemData WeaponData;
    public ItemData HelmData;
    public ItemData ArmourData;
    public ItemData ConsumableData;
    public int DamageType;
    public int SpriteIndex;
    public float Level;
    public float MaxHP;
    public float MaxXP;
    public float XP;
    public float Damage;
    public float Range;
    public float Discovery;
    public float LootFind;
    public float Timer;
    public bool Training;
    
}
