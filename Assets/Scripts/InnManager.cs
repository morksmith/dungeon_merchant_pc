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
    public float merchantTimer;
    public float heroTimer;
    public float requestTimer;
    public ScrollingWindow BottomContent;
    public ScrollingWindow TopContent;
    public InnData Data;
    public AudioSource Audio;

    private void Start()
    {

    }

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
            Audio.volume = PlayerPrefs.GetFloat("SFX Volume");
            Audio.Play();
            NewMerchant();
            BottomContent.NewMerchantIcon();
            merchantTimer = 0;
        }
        if (heroTimer > HeroTime)
        {
            Audio.volume = PlayerPrefs.GetFloat("SFX Volume");
            Audio.Play();
            NewHero();
            BottomContent.NewHeroIcon();
            heroTimer = 0;
        }
    }
    public void NewMerchant()
    {
        var merchLevel = (Stock.MaxProfit / 1000);
        merchLevel = Mathf.CeilToInt(merchLevel);
        int merchInt = (int)merchLevel;
        Merchant.NewItems(merchInt);
        Merchant.GetComponent<Menu>().DeActivate();

    }
    public void NewHero()
    {
        Hire.NewHero();
    }

    public void SetTimers(float m, float h, float r)
    {
        merchantTimer = m;
        heroTimer = h;
        requestTimer = r;
    }
    public void NewDay()
    {
        merchantTimer++;
        heroTimer++;
        requestTimer++;
    }

    public void StoreData()
    {
        var newData = new InnData();
        newData.MerchTime = merchantTimer;
        newData.HeroTime = heroTimer;
        newData.RequestTime = requestTimer;
        if (Hire.HeroAvailable)
        {
            if(Hire.CurrentHero != null)
            {
                newData.HireHero = Hire.CurrentHero.GetComponent<Stats>().Data;
            }
        }
        newData.HeroAvailable = Hire.HeroAvailable;
        Data = newData;
    }
}
