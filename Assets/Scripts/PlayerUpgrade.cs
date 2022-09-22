using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class PlayerUpgrade : MonoBehaviour
{
    public DungeonManager DM;
    public int Level;
    public Menu UpgradeMenu;
    public Stats HeroStats;
    public string UpgradeName;
    public TextMeshProUGUI ButtonText;
    public float Value;
    public float Cost;
    public Button UpgradeButton;
    public bool Paid = false;
    public Color UncommonColour;
    public Color RareColour;

    public enum UpgradeType
    {
        HP,
        MaxHP,
        Speed,
        Range,
        Damage,
    }
    public UpgradeType Type;
    public int Rarity = 0;

    private void Start()
    {
        SetRarity();
    }

    public void SelectUpgrade()
    {
        if (Paid)
        {
            HeroStats.GoldHeld -= Cost;
        }
        if (Type == UpgradeType.HP)
        {
            HeroStats.HP += ((HeroStats.MaxHP/100) * Value);
            HeroStats.HP = Mathf.Clamp(HeroStats.HP, 0, HeroStats.MaxHP);
        }
        if (Type == UpgradeType.MaxHP)
        {
            var currentPerc = HeroStats.HP / HeroStats.MaxHP;
            HeroStats.MaxHP += Value;
            HeroStats.HP = currentPerc * HeroStats.MaxHP;
        }
        if (Type == UpgradeType.Speed)
        {
            DM.CurrentHeroAI.GetComponent<NavMeshAgent>().speed += Value / 10;
        }
        if (Type == UpgradeType.Range)
        {
            HeroStats.Range += Value /10;
        }
        if (Type == UpgradeType.Damage)
        {
            HeroStats.Damage += Value;
        }
        UpgradeMenu.DeActivate();
        DM.CurrentHeroAI.Waiting = false;
        DM.Running = true;
    }

    public void SetRarity()
    {
        Level = DM.Level;
        var pickRarity = Random.Range(0, 21);
        if (pickRarity > 0)
        {
            Rarity = 0;
            UpgradeButton.image.color = Color.white;
        }
        if (pickRarity > 15)
        {
            Rarity = 1;
            UpgradeButton.image.color = UncommonColour;
        }
        if (pickRarity > 19)
        {
            Rarity = 2;
            UpgradeButton.image.color = RareColour;
        }
        if (Type == UpgradeType.HP)
        {
            Value = 25 + Rarity * 50;
            Value = Mathf.Clamp(Value, 25, 100);
            ButtonText.text = "Restore " + Value + "% HP";
        }
        else if (Type == UpgradeType.MaxHP)
        {
            Value = 5 + 10 * Rarity;
            ButtonText.text = "Max HP +" + Value;
        }
        else if (Type == UpgradeType.Speed)
        {
            Value = 0.5f + 1 * Rarity;
            ButtonText.text = "Move Speed +" + Value;
        }
        else if (Type == UpgradeType.Range)
        {
            Value = 0.5f + 0.5f * Rarity;
            ButtonText.text = "Range +" + Value;
        }
        else if (Type == UpgradeType.Damage)
        {
            Value = 5 + 5 * Rarity;
            ButtonText.text = "Damage +" + Value;
        }
        if (Paid)
        {
            Cost = (10 * Level) + (Rarity * 50);
            ButtonText.text += " (" + Cost + "G)";
            if(HeroStats.GoldHeld < Cost)
            {
                GetComponent<Button>().interactable = false;
            }
            else
            {
                GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }

    }
}
