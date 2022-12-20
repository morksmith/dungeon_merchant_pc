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
    public GameObject TrainBlock;
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
        if (!Stock.SurvivalMode)
        {
            if (AllHeroes[CurrentHero].State == Stats.HeroState.Training)
            {
                TrainBlock.SetActive(true);
            }
            else
            {
                TrainBlock.SetActive(false);
            }
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
        var weaponBonus = CalculateWeaponBonus(s);
        HeroInfoText.text = s.HeroName + "\nLevel " + s.Level + " " + s.Class + "\nHP:" + s.MaxHP + "\nXP:" + s.XP + "/" + s.MaxXP + "\nDamage:" + Mathf.CeilToInt(s.Damage * weaponBonus) + "\nRange:" + Mathf.FloorToInt(s.Range) + "\nGold Drop:x" + s.Discovery;
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

    public float CalculateWeaponBonus(Stats s)
    {
        var WeaponBonus = 0f;
        if (s.ArmourItem != null)
        {
            if (s.ArmourItem.Special)
            {
                if (s.Class == Stats.HeroClass.Ranger)
                {
                    if (s.ArmourItem.ArmourBonus == Item.ArmourBonusType.Bow)
                    {
                        WeaponBonus = s.ArmourItem.BonusStat;
                    }
                }
                else if (s.Class == Stats.HeroClass.Warrior)
                {
                    if (s.ArmourItem.ArmourBonus == Item.ArmourBonusType.Club)
                    {
                        WeaponBonus = s.ArmourItem.BonusStat;
                    }
                }
                else if (s.Class == Stats.HeroClass.Knight)
                {
                    if (s.ArmourItem.ArmourBonus == Item.ArmourBonusType.Sword)
                    {
                        WeaponBonus = s.ArmourItem.BonusStat;
                    }
                }
                else if (s.Class == Stats.HeroClass.Mage)
                {
                    if (s.ArmourItem.ArmourBonus == Item.ArmourBonusType.Wand)
                    {
                        WeaponBonus = s.ArmourItem.BonusStat;
                    }
                }
            }
        }
        var HelmBonus = 0f;
        if (s.HelmItem != null)
        {
            
            if (s.HelmItem.Special)
            {
                if (s.Class == Stats.HeroClass.Ranger)
                {
                    if (s.HelmItem.ArmourBonus == Item.ArmourBonusType.Bow)
                    {
                        HelmBonus = s.HelmItem.BonusStat;
                    }
                }
                else if (s.Class == Stats.HeroClass.Warrior)
                {
                    if (s.HelmItem.ArmourBonus == Item.ArmourBonusType.Club)
                    {
                        HelmBonus = s.HelmItem.BonusStat;
                    }
                }
                else if (s.Class == Stats.HeroClass.Knight)
                {
                    if (s.HelmItem.ArmourBonus == Item.ArmourBonusType.Sword)
                    {
                        HelmBonus = s.HelmItem.BonusStat;
                    }
                }
                else if (s.Class == Stats.HeroClass.Mage)
                {
                    if (s.HelmItem.ArmourBonus == Item.ArmourBonusType.Wand)
                    {
                        HelmBonus = s.HelmItem.BonusStat;
                    }
                }
            }
            WeaponBonus += HelmBonus;
            
        }
        if (WeaponBonus + HelmBonus == 0)
        {
            WeaponBonus = 1;
        }

        return (WeaponBonus);
    }


}
