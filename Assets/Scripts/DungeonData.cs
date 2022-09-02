using UnityEngine;
using System;
[Serializable]
public class DungeonData
{
    public float Time;
    public int EnemyCount;
    public int NextCount;
    public int EnemyStrength;
    public int NextStrength;
    public int[] SpawnTypes;
    public float SleepTimer;
}
