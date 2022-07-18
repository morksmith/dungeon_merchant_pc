using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnManager : MonoBehaviour
{
    public StockManager Stock;
    public HeroManager HeroManager;
    public float MerchantTime = 2;
    public float HeroTime = 3;
    public float RequestTime = 4;
    public MerchantMenu Merchant;
    public HireMenu Hire;
    public RequestManager Requests;
    private float merchantTimer;
    private float heroTimer;
    private float requestTimer;
    public ScrollingWindow BottomContent;
    public ScrollingWindow TopContent;

    private void Update()
    {

        if (requestTimer > RequestTime)
        {
            Requests.NewRequests();
            TopContent.NewMerchantIcon();
            requestTimer = 0;
        }
        if (merchantTimer > MerchantTime)
        {
            NewMerchant();
            BottomContent.NewMerchantIcon();
            merchantTimer = 0;
        }
        if (heroTimer > HeroTime)
        {
            NewHero();
            BottomContent.NewHeroIcon();
            heroTimer = 0;
        }
    }
    public void NewMerchant()
    {
        var merchLevel = (Stock.MaxProfit / 100) / 4;
        merchLevel = Mathf.CeilToInt(merchLevel);
        int merchInt = (int)merchLevel;
        Debug.Log(merchLevel + ">" + merchInt);
        Merchant.NewItems(merchInt);
        Merchant.GetComponent<Menu>().DeActivate();

    }
    public void NewHero()
    {
        Hire.NewHero();
    }

    public void NewDay()
    {
        merchantTimer++;
        heroTimer++;
        requestTimer++;
    }
}
