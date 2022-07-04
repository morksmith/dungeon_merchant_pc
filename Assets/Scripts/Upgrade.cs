using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Button PurchaseButton;
    public GameObject CompleteObject;
    public float Price;
    public StockManager Stock;
    public bool Complete = false;

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
    }
    public void BuyShelves()
    {
        Stock.ShopSlots = 8;
        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
    }
    public void BuySellSpeed()
    {
        Stock.SellSpeed = 2;
        Stock.CollectGold(-Price);
        CompleteObject.SetActive(true);
        Complete = true;
        PurchaseButton.interactable = false;
    }
}
