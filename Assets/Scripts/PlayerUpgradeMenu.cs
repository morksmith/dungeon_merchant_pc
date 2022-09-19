using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeMenu : MonoBehaviour
{
    public List<GameObject> Upgrades;
    public Menu UpgradeMenu;

    private void Start()
    {
        NewUpgrades();
    }
    public void NewUpgrades()
    {
        UpgradeMenu.Activate();
        foreach(GameObject go in Upgrades)
        {
            go.SetActive(false);
        }

        var upgrade1 = Random.Range(0, Upgrades.Count);
        var upgrade2 = Random.Range(0, Upgrades.Count);
        while(upgrade2 == upgrade1)
        {
            upgrade2 = Random.Range(0, Upgrades.Count);
        }
        var upgrade3 = Random.Range(0, Upgrades.Count);
        while(upgrade3 == upgrade1)
        {
            upgrade3 = Random.Range(0, Upgrades.Count);
            while(upgrade3 == upgrade2)
            {
                upgrade3 = Random.Range(0, Upgrades.Count);
            }
        }

        Upgrades[upgrade1].SetActive(true);
        Upgrades[upgrade1].GetComponent<PlayerUpgrade>().SetRarity();
        Upgrades[upgrade2].SetActive(true);
        Upgrades[upgrade2].GetComponent<PlayerUpgrade>().SetRarity();
        Upgrades[upgrade3].SetActive(true);
        Upgrades[upgrade3].GetComponent<PlayerUpgrade>().SetRarity();
    }
}
