using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class PlayerUpgrade : MonoBehaviour
{
    public DungeonManager DM;
    public Menu UpgradeMenu;
    public Stats HeroStats;
    public string UpgradeName;
    public TextMeshProUGUI ButtonText;
    public float Value;
    public Button UpgradeButton;

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
        if (Type == UpgradeType.HP)
        {
            HeroStats.HP += (HeroStats.MaxHP * Value);
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
            HeroStats.Range += Value;
        }
        if (Type == UpgradeType.Damage)
        {
            HeroStats.Damage += Value;
        }
        DM.Running = true;
        UpgradeMenu.DeActivate();
    }

    public void SetRarity()
    {
        var pickRarity = Random.Range(0, 21);
        if (pickRarity > 0)
        {
            Rarity = 0;
            UpgradeButton.image.color = Color.white;
        }
        if (pickRarity > 15)
        {
            Rarity = 1;
            UpgradeButton.image.color = Color.blue;
        }
        if (pickRarity > 19)
        {
            Rarity = 2;
            UpgradeButton.image.color = Color.yellow;
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
    }
}
