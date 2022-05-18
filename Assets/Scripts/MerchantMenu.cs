using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MerchantMenu : MonoBehaviour
{
    public StockManager Stock;
    public TextMeshProUGUI ItemInfo;
    public Item SelectedItem;
    public ItemGenerator Generator;

    private void Start()
    {
        NewItems(1);
    }
    public void SelectItem(Item i)
    {
        
        if(i.GetComponent<Weapon>() != null)
        {
            ItemInfo.text = i.GetComponent<Weapon>().WeaponName + " (" + i.GetComponent<Weapon>().Level + ")" + "\nDMG: " + i.GetComponent<Weapon>().Damage + "\n" + i.Price + "G";
        }
        SelectedItem = i;
    }

    public void BuyItem()
    {
        if(Stock.Gold >= SelectedItem.Price)
        {
            SelectedItem.transform.SetParent(Stock.StockList);
            SelectedItem.Merchant = false;
            Stock.CollectGold(-SelectedItem.Price);
            SelectedItem = null;
            ItemInfo.text = "SELECT AN ITEM";
            Stock.UpdatePrices();
        }
        else 
        {
            ItemInfo.text = "YOU DON'T HAVE ENOUGH GOLD!";
        }
        
    }

    public void RemoveItems()
    {
        var currentItems = GetComponentsInChildren<Item>();
        for(var i = 0; i < currentItems.Length; i++)
        {
            Destroy(currentItems[i].gameObject);
        }
    }

    public void NewItems(int l)
    {
        RemoveItems();
        var itemLevel = Random.Range(1, l + 1);
        for(var i = 0; i <6; i++)
        {
            Generator.GenerateWeapon(itemLevel, true);
        }
        Stock.UpdatePrices();
        

    }
}
