using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HireMenu : MonoBehaviour
{
    public bool Tutorial = false;
    public bool HeroAvailable = false;
    public float HeroCost;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI HeroText;
    public TextMeshProUGUI PriceText;
    public Menu HireScreen;
    public Transform HeroParent;
    public Transform CurrentHero;
    public StockManager Stock;
    public GameObject HeroButton;
    public Image HeroIcon;
    public Image HeroSprite;
    public HeroGenerator Generator;
    public ScrollingWindow DungeonContent;
    public GameObject NewIcon;
    public SaveManager Save;
    public HeroData HiredHero;
    public AudioClip HireHeroSound;
    private SFXManager sfx;

    private void Start()
    {
        sfx = GameObject.FindObjectOfType<SFXManager>();
        //NewHero();
    }
    public void HireHero()
    {
        if(HeroCost <= Stock.Gold)
        {
            sfx.PlaySound(HireHeroSound);
            CurrentHero.gameObject.SetActive(true);
            CurrentHero.SetParent(HeroParent);
            CurrentHero.transform.localScale = Vector3.one;
            CurrentHero.GetComponent<Stats>().State = Stats.HeroState.Idle;
            CurrentHero.GetComponent<Stats>().StoreData();
            CurrentHero = null;
            HireScreen.DeActivate();
            HeroButton.SetActive(false);
            Stock.CollectGold(-HeroCost);
            HeroAvailable = false;
            if (!Tutorial)
            {
                Save.SaveGame();
            }
        }
        else
        {
            HeroText.text = "YOU CAN'T AFFORD ME!";
        }
        
        


    }

    public void UpdateHeroInfo(Transform hero)
    {
        CurrentHero = hero;
        var s = CurrentHero.GetComponent<Stats>();
        HeroCost = 50  + s.Level * 50;
        PriceText.text = HeroCost + "G";
        NameText.text = s.HeroName;
        HeroText.text = "Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + " / " + s.MaxXP + "\n Damage:" + s.Damage + "\n Range:" + Mathf.FloorToInt(s.Range) + "\n Gold Drop: x" + s.Discovery;
        HeroIcon.sprite = s.HeroSprite;
        HeroSprite.sprite = s.HeroSprite;
        s.StoreData();

    }

    public void NewHero()
    {
        HeroButton.SetActive(true);
        HireScreen.DeActivate();
        if(CurrentHero != null)
        {
            Destroy(CurrentHero.gameObject);
        }
        Generator.CreateHero();
        NewIcon.SetActive(true);
        HeroAvailable = true;

    }
    public void LoadHireHero(HeroData hd)
    {
        HeroButton.SetActive(true);
        HireScreen.DeActivate();
        if (CurrentHero != null)
        {
            Destroy(CurrentHero.gameObject);
        }
        Generator.CreateHiredHero(hd);
        NewIcon.SetActive(true);
        HeroAvailable = true;

    }
    public void CreateTutorialHero()
    {
        HeroButton.SetActive(true);
        HireScreen.DeActivate();
        if (CurrentHero != null)
        {
            Destroy(CurrentHero.gameObject);
        }
        Generator.CreateTutorialHero();
        NewIcon.SetActive(true);
    }

   

}
