using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret : MonoBehaviour
{
    public float CatCount;
    public HireMenu Hire;
    public float CatTimeout;
    private float timer; 
    void Start()
    {
        CatCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Survival Mode") == 1)
        {
            return;
        }
        if (CatCount > 0)
        {
            timer += Time.deltaTime;
            if(timer > CatTimeout)
            {
                timer = 0;
                CatCount = 0;
            }

        }
    }

    public void ClickCat()
    {
        if(PlayerPrefs.GetInt("Survival Mode") == 1)
        {
            return;
        }
        CatCount++;
        timer = 0;
        if(CatCount > 7)
        {
            if (Hire.CurrentHero.GetComponent<Stats>().Class == Stats.HeroClass.Knight)
            {
                Hire.CurrentHero.GetComponent<Stats>().HeroName = "Kitten";
                Hire.UpdateHeroInfo(Hire.CurrentHero);
                timer = 0;
                CatCount = 0;
            }
        }
        
    }
}
