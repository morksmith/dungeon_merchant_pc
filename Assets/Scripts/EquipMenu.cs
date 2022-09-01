using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    public GameObject EquipBlock;
    public Stats[] AllHeroes;
    public Transform WeaponItemSlot;
    public Transform HelmSlot;
    public Transform ArmourSlot;
    public Transform ConsumableSlot;
    public void UpdateEquipMenu(Stats s)
    {
        AllHeroes = GameObject.FindObjectsOfType<Stats>().Where(h => h.State != Stats.HeroState.NotHired).ToArray();
        for (var i = 0; i < AllHeroes.Length; i++)
        {
            if(AllHeroes[i] == s)
            {
                CurrentHero = i;
            }
        }
        if(AllHeroes[CurrentHero].State == Stats.HeroState.Questing)
        {
            EquipBlock.SetActive(true);
        }
        else
        {
            EquipBlock.SetActive(false);
        }
        WeaponImage.sprite = WeaponSprites[s.DamageType];
        WeaponSlot.DamageType = s.DamageType;
        if(s.DamageType == 0)
        {
            WeaponItemSlot.GetComponent<ItemType>().Type = ItemType.ItemTypes.Sword;
        }
        else if(s.DamageType == 1)
        {
            WeaponItemSlot.GetComponent<ItemType>().Type = ItemType.ItemTypes.Wand;
        }
        else if (s.DamageType == 2)
        {
            WeaponItemSlot.GetComponent<ItemType>().Type = ItemType.ItemTypes.Bow;
        }
        else if (s.DamageType == 3)
        {
            WeaponItemSlot.GetComponent<ItemType>().Type = ItemType.ItemTypes.Club;
        }

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
