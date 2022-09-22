using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUpgradeMenu : MonoBehaviour
{
    public List<GameObject> Upgrades;
    public Menu UpgradeMenu;
    public DungeonManager DM;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI BodyText;

    private void Start()
    {
        //NewUpgrades(true);
        //DM.Running = false;
    }
    public void NewUpgrades(bool merchant)
    {
        DM.Running = false;
        DM.CurrentHeroAI.Waiting = true;
        UpgradeMenu.Activate();
        if (merchant)
        {
            TitleText.text = "MERCHANT FOUND!";
            BodyText.text = "Upgrades for sale!";
        }
        else
        {
            TitleText.text = "LEVEL UP!";
            BodyText.text = "Select an upgrade";
        }
        foreach(GameObject go in Upgrades)
        {
            go.GetComponent<PlayerUpgrade>().Paid = merchant;
            go.SetActive(false);
        }

        List<int> options = new List<int>() { 0, 1, 2, 3, 4};

        var pick1 = Random.Range(0, options.Count);
        int uniqueNumber1 = options[pick1];
        options.RemoveAt(pick1);
        var pick2 = Random.Range(0, options.Count);
        int uniqueNumber2 = options[pick2];
        options.RemoveAt(pick2);
        var pick3 = Random.Range(0, options.Count);
        int uniqueNumber3 = options[pick3];
        options.RemoveAt(pick3);

        Debug.Log("Picked: " + uniqueNumber1 + "-" + uniqueNumber2 + "-" + uniqueNumber3 + "\n Left Overs: " + options);
        foreach (GameObject go in Upgrades)
        {
            go.GetComponent<PlayerUpgrade>().Paid = merchant;
        }
        Upgrades[uniqueNumber1].SetActive(true);
        Upgrades[uniqueNumber1].GetComponent<PlayerUpgrade>().SetRarity();
        Upgrades[uniqueNumber2].SetActive(true);
        Upgrades[uniqueNumber2].GetComponent<PlayerUpgrade>().SetRarity();
        Upgrades[uniqueNumber3].SetActive(true);
        Upgrades[uniqueNumber3].GetComponent<PlayerUpgrade>().SetRarity();
        
    }

    public void CloseMenu()
    {
        UpgradeMenu.DeActivate();
        DM.Running = true;
        foreach (GameObject go in Upgrades)
        {
            go.GetComponent<Button>().interactable = false;
        }
    }
}
