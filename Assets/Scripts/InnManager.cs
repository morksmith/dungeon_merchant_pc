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
    public MerchantMenu Merchant;
    public HireMenu Hire;
    private float merchantTimer;
    private float heroTimer;
    public ScrollingWindow BottomContent;

    private void Update()
    {
        

        if(merchantTimer > MerchantTime)
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
        var merchLevel = (Stock.MaxProfit / 100) / 2;
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
    }
}
