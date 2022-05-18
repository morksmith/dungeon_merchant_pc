using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MerchantMenu : MonoBehaviour
{
    public StockManager Stock;
    public TextMeshProUGUI ItemInfo;
    public Item SelectedItem;

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
        }
        else 
        {
            ItemInfo.text = "YOU DON'T HAVE ENOUGH GOLD!";
        }
        
    }
}
