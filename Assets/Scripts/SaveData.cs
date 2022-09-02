using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SaveData
{
    public float Gold;
    public float MaxProfit;
    public int MaxLevel;
    public List<HeroData> AllHeroes;
    public List<ItemData> AllItems;
    public DungeonData DungeonSaveData;
    public InnData InnSaveData;
    public HeroData HireHero;
}
