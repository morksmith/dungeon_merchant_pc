using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockManager : MonoBehaviour
{
    public float Gold;
    public Item CurrentItem;
    public TextMeshProUGUI ItemInfoText;
    public TextMeshProUGUI GoldText;
    public Transform StockList;
    public Transform ShopList;
    public List<Item> ItemList;
    public float SwordPrice = 1;
    public float ClubPrice = 1;
    public float BowPrice = 1;
    public float WandPrice = 1;
    public float ArmourPrice = 1;
    public float PotionPrice = 1;
    public int ShopSlots = 4;
    // Start is called before the first frame update
    void Start()
    {
        Gold = PlayerPrefs.GetFloat("Gold");
        GoldText.text = Gold + "G";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SellItem()
    {
        if(CurrentItem != null)
        {
            if(ShopList.childCount < ShopSlots)
            {
                CurrentItem.transform.parent = ShopList;
                CurrentItem.Selling = true;
            }
            else
            {
                Debug.Log("No available slots in shop");
            }

        }
        else
        {
            Debug.Log("No Item selected");
        }
    }

    public void SelectItem(Item i)
    {
        CurrentItem = i;
        if(i.Type == Item.ItemType.Weapon)
        {
            ItemInfoText.text = i.GetComponent<Weapon>().WeaponName + "\n\n DMG: " + i.GetComponent<Weapon>().Damage + "\n\n" + i.Price +"G"; 
        }
    }

    public void CollectGold(float i)
    {
        Gold += i;
        GoldText.text = Gold + "G";
        PlayerPrefs.SetFloat("Gold", Gold);
    }

    public void UpdatePrices()
    {
        
        for(var i = 0; i < ItemList.Count; i++)
        {
            if(ItemList[i].Type == Item.ItemType.Weapon)
            {
                if(ItemList[i].GetComponent<Weapon>().Type == Weapon.WeaponType.Sword)
                {
                    ItemList[i].Price *= SwordPrice;
                }
            }
        }
    }
}
