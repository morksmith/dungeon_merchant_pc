using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        Weapon,
        Armour,
        Consumable
    }
    public ChestType Type;
    public int ItemType;
    public int Level = 1;
    private StockManager stock;

    private void Start()
    {
        stock = GameObject.FindObjectOfType<StockManager>(); 
        if(Type == ChestType.Armour)
        {
            ItemType = 0;
        }
        else if(Type == ChestType.Weapon)
        {
            ItemType = 1;
        }
        else if (Type == ChestType.Consumable)
        {
            ItemType = 3;
        }
    }

    public void PickRandom()
    {
        var i = Random.Range(0, 3);
        if(i == 0)
        {
            Type = ChestType.Weapon;
            ItemType = 0;
        }
        else if (i == 1)
        {
            Type = ChestType.Armour;
            ItemType = 1;
        }
        else if (i == 3)
        {
            Type = ChestType.Consumable;
            ItemType = 3;
        }
    }

    public void CreateItem()
    {
        stock.CreateItem(ItemType, Level);
        stock.UpdatePrices();
        Destroy(gameObject);
        
    }


}
