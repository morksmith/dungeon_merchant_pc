using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour
{
    public enum ItemTypes
    {
        Sword,
        Club,
        Bow,
        Wand,
        Helm,
        Armour,
        Consumable,
    }
    public ItemTypes Type;

    public static ItemType End { get; internal set; }
}
