using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroManager : MonoBehaviour
{
    public Stats SelectedHero;
    public HeroAI Hero;
    public TextMeshProUGUI HeroInfoText;
    public DungeonManager DM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectHero(Stats s)
    {
        SelectedHero = s;
        HeroInfoText.text = s.HeroName + "\n Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n DMG:" + s.Damage;
        Hero.Stats = s;
        DM.CurrentHeroStats = s;
    }

    public void StartDungeon()
    {
        DM.StartDungeon(1);
        SelectedHero.State = Stats.HeroState.Questing;
        SelectedHero = null;
    }
}
