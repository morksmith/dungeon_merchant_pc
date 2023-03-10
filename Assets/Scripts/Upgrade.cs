using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Button PurchaseButton;
    public GameObject CompleteObject;
    public SingleSound Sound;

    public enum UpgradeType
    {
        Discount,
        Shelves,
        Speed,
        Speed2,
        RequestPrice,
        MagicChest

    }
    public UpgradeType Type;
    public float Price;
    public StockManager Stock;
    public bool Complete = false;
    public GameObject Shelves;

    private void Start()
    {
        if (Type == UpgradeType.Discount)
        {
            if(PlayerPrefs.GetInt("Discount") == 1)
            {
                Stock.EnableMerchDiscount();
                CompleteObject.SetActive(true);
                Complete = true;
                PurchaseButton.interactable = false;
            }
        }
        else if (Type == UpgradeType.Shelves)
        {
            if (PlayerPrefs.GetInt("Shelves") == 1)
            {
                Stock.ShopSlots = 8;
                CompleteObject.SetActive(true);
                Complete = true;
                PurchaseButton.interactable = false;
                Shelves.SetActive(true);
            }
        }
        else if (Type == UpgradeType.Speed)
        {
            if(PlayerPrefs.GetInt("Sell Speed") == 1)
            {
                Stock.SellSpeed = 2;
                CompleteObject.SetActive(true);
                Complete = true;
                PurchaseButton.interactable = false;
            }
        }
        else if (Type == UpgradeType.Speed2)
        {
            if (PlayerPrefs.GetInt("Sell Speed 2") == 1)
            {
                if (Stock.SellSpeed == 1)
                {
                    Stock.SellSpeed = 2;
                }
                else if(Stock.SellSpeed == 2)
                {
                    Stock.SellSpeed = 4;
                }
                
                CompleteObject.SetActive(true);
                Complete = true;
                PurchaseButton.interactable = false;
            }
        }
        else if (Type == UpgradeType.RequestPrice)
        {
            if (PlayerPrefs.GetInt("Request Bonus") == 1)
            {
                Stock.RequestBonus = 1.5f;
                CompleteObject.SetActive(true);
                Complete = true;
                PurchaseButton.interactable = false;
            }
        }
        else if (Type == UpgradeType.MagicChest)
        {
            if (PlayerPrefs.GetInt("MagicChestUnlocked") == 1)
            {
                CompleteObject.SetActive(true);
                Complete = true;
                PurchaseButton.interactable = false;
            }
        }
    }

    public void Update()
    {
        if(Complete == true)
        {
            return;
        }
        if(Stock.Gold < Price)
        {
            PurchaseButton.interactable = false;
        }
        else
        {
            PurchaseButton.interactable = true;
        }
    }
    public void BuyDiscount()
    {
        Stock.EnableMerchDiscount();
        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
        PlayerPrefs.SetInt("Discount", 1);

    }
    public void BuyShelves()
    {
        Stock.ShopSlots = 8;
        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
        PlayerPrefs.SetInt("Shelves", 1);
        Shelves.SetActive(true);

    }
    public void BuySellSpeed()
    {
        if (Stock.SellSpeed == 1)
        {
            Stock.SellSpeed = 2;
        }
        else if (Stock.SellSpeed == 2)
        {
            Stock.SellSpeed = 4;
        }
        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
        PlayerPrefs.SetInt("Sell Speed", 1);

    }
    public void BuySellSpeed2()
    {
        if (Stock.SellSpeed == 1)
        {
            Stock.SellSpeed = 2;
        }
        else if(Stock.SellSpeed == 2)
        {
            Stock.SellSpeed = 4;
        }

        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
        PlayerPrefs.SetInt("Sell Speed 2", 1);

    }
    public void BuyRequest()
    {
        Stock.RequestBonus = 1.5f;
        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
        PlayerPrefs.SetInt("Request Bonus", 1);
    }
    public void BuyMagicChest()
    {
        Stock.BuyMagicChest();
        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
        PlayerPrefs.SetInt("MagicChestUnlocked", 1);
    }


    public void BuyUpgrade()
    {
        Sound.PlaySound();

        if (Type == UpgradeType.Discount)
        {
            BuyDiscount();
        }
        else if(Type == UpgradeType.Shelves)
        {
            BuyShelves();
        }
        else if(Type == UpgradeType.Speed)
        {
            BuySellSpeed();
        }
        else if (Type == UpgradeType.Speed2)
        {
            BuySellSpeed2();
        }
        else if (Type == UpgradeType.RequestPrice)
        {
            BuyRequest();
        }
        else if (Type == UpgradeType.MagicChest)
        {
            BuyMagicChest();
        }
    }
   

}
