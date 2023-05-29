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
    public GameObject NewIcon;
    public bool Tutorial = false;

    private void Start()
    {
        


    }
    public void SelectItem(Item i)
    {
        
        if(i.GetComponent<Weapon>() != null)
        {
            ItemInfo.text = i.GetComponent<Weapon>().WeaponName + " (" + i.GetComponent<Weapon>().Level + ")" + "\nDMG: " + i.GetComponent<Weapon>().Damage + "\n" + i.BonusString + "\n" + i.Price + "G" ;
        }
        if (i.GetComponent<Armour>() != null)
        {
            ItemInfo.text = i.ItemName + " (" + i.GetComponent<Armour>().Level + ")" + "\n+HP: " + i.GetComponent<Armour>().HP + "\n" + i.BonusString + "\n" + i.Price + "G";
        }
        if (i.GetComponent<Consumable>() != null)
        {
            {
                if (i.GetComponent<Consumable>().Type == Consumable.ConsumableType.Potion)
                {
                    ItemInfo.text = i.ItemName + "\n+" + i.GetComponent<Consumable>().Value + "HP"+ "\n" + i.Price + "G";
                }
                else if (i.GetComponent<Consumable>().Type == Consumable.ConsumableType.Portal)
                {
                    ItemInfo.text = i.ItemName + "\n Returns hero safely" + "\n" + i.Price + "G";
                }
                else if (i.GetComponent<Consumable>().Type == Consumable.ConsumableType.Damage)
                {
                    ItemInfo.text = i.ItemName + "\n 2x Damage" + "\n" + i.GetComponent<Consumable>().Value + " Hits" + "\n" + i.Price + "G";
                }


            }
        }
        SelectedItem = i;
        if (i.Special)
        {
            ItemInfo.color = i.SpecialColour;
        }
        else
        {
            ItemInfo.color = Color.white;
        }
    }

    public void BuyItem()
    {
        if(SelectedItem == null)
        {
            return;
        }
        if(Stock.Gold >= SelectedItem.Price)
        {
            SelectedItem.transform.SetParent(Stock.StockList);
            SelectedItem.Merchant = false;
            Stock.CollectGold(-SelectedItem.Price);
            var stat = new Steamworks.Data.Stat("gold_spent");
            int spend = (int)SelectedItem.Price;
            Debug.Log(spend);
            stat.Add(spend);
            SelectedItem = null;
            ItemInfo.text = "SELECT AN ITEM";
            ItemInfo.color = Color.white;
            Stock.UpdatePrices();
            Stock.PlaySellSound();
            
        }
        else 
        {
            ItemInfo.text = "YOU DON'T HAVE ENOUGH GOLD!";
            ItemInfo.color = Color.white;
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
        
        for(var i = 0; i <4; i++)
        {
            var itemLevel = Random.Range(1, l + 2);
            var pick = Random.Range(0, 3);
            if(pick == 0)
            {
                Generator.GenerateWeapon(itemLevel, true);
            }
            else if (pick == 1)
            {
                Generator.GenerateArmour(itemLevel, true);
            }
            else if (pick == 2)
            {
                Generator.GenerateConsumable(itemLevel, true);
            }
        }
        Stock.UpdatePrices();
        NewIcon.SetActive(true);
        

    }
}
