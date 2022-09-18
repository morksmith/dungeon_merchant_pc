using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public string UpgradeName;
    public float Value;
    public enum UpgradeType
    {
        HP,
        MaxHP,
        Speed,
        Range,
        Damage,
    }
}
