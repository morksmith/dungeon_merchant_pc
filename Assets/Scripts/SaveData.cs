using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public StockManager Stock;
    public List<Item> StockItems;

    public void CollectData()
    {
        StockItems.Clear();
        foreach(Item i in Stock.AllItems)
        {
            if(!i.Merchant && !i.Equipped)
            {
                StockItems.Add(i);
            }
        }
    }
}
