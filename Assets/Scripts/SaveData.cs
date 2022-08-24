using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public StockManager Stock;
    public List<Item> StockItems;

    private void Start()
    {
        //TestSaving();
    }
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

    public async void TestSaving()
    {
        var itemData = new ItemData();
        itemData.ItemName = "CoolItem";
        await DungeonMerchant.FileIO.JsonSerializationHandler.ResolveDataDirectoryAsync();
        await DungeonMerchant.FileIO.JsonSerializationHandler.SerializeObjectToDataDirectory(itemData, "SaveData.json");

    }
}
