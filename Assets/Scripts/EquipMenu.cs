using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipMenu : MonoBehaviour
{
    public Image HeroImage;
    public Image WeaponImage;
    public HeroManager Heroes;
    public StockManager Stock;
    public int CurrentHero = 0;
    public List<Sprite> WeaponSprites;
    public TextMeshProUGUI HeroInfoText;
    public EquipmentSlot WeaponSlot;
    public Stats[] AllHeroes;

    public void UpdateEquipMenu(Stats s)
    {
        AllHeroes = GameObject.FindObjectsOfType<Stats>();
        for (var i = 0; i < AllHeroes.Length; i++)
        {
            if(AllHeroes[i] == s)
            {
                CurrentHero = i;
            }
        }
        WeaponImage.sprite = WeaponSprites[s.DamageType];
        WeaponSlot.DamageType = s.DamageType;
        HeroInfoText.text = s.HeroName + "\n Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + "/" + s.MaxXP + "\n Damage:" + s.Damage + "\n Range:" + Mathf.FloorToInt(s.Range) + "\n Gold Drop:x" + s.Discovery;
        HeroImage.sprite = s.HeroSprite;
    }

    public void NextHero()
    {
        if (AllHeroes.Length == 0)
        {
            CurrentHero = 0;
        }
        else
        {
            if (CurrentHero < AllHeroes.Length - 1)
            {
                CurrentHero++;
            }
            else
            {
                CurrentHero = 0;
            }
        }
        
        Heroes.SelectHero(AllHeroes[CurrentHero]);
        AllHeroes[CurrentHero].SelectHero();
        UpdateEquipMenu(AllHeroes[CurrentHero]);
        if (Stock.CurrentItem != null)
        {
            Stock.SelectItem(Stock.CurrentItem);
        }

    }
    public void PreviousHero()
    {
        if (AllHeroes.Length == 0)
        {
            CurrentHero = 0;
        }
        else
        {
            if (CurrentHero > 0)
            {
                CurrentHero--;
            }
            else
            {
                CurrentHero = AllHeroes.Length - 1;
            }
        }
        
        Heroes.SelectHero(AllHeroes[CurrentHero]);
        AllHeroes[CurrentHero].SelectHero();
        UpdateEquipMenu(AllHeroes[CurrentHero]);
        

    }


}
