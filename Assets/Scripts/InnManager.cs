using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnManager : MonoBehaviour
{
    public StockManager Stock;
    public HeroManager HeroManager;
    public float MerchantTime = 120;
    public float HeroTime = 140;
    public MerchantMenu Merchant;
    private float merchantTimer;
    private float heroTimer;

    private void Update()
    {
        heroTimer += Time.deltaTime;
        merchantTimer += Time.deltaTime;

        if(merchantTimer > MerchantTime)
        {
            NewMerchant();
            merchantTimer = 0;
        }
    }
    public void NewMerchant()
    {
        var merchLevel = Stock.MaxProfit / 100;
        merchLevel = Mathf.CeilToInt(merchLevel);
        int merchInt = (int)merchLevel;
        Debug.Log(merchLevel + ">" + merchInt);
        Merchant.NewItems(merchInt);
        Merchant.GetComponent<Menu>().DeActivate();

    }
}
