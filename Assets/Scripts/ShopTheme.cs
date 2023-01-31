using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTheme : MonoBehaviour
{
    public Color NormalColour;
    public Color AppliedColour;
    public ThemeManager TM;
    public Image Background;
    public GameObject ThemeOverlay;
    public bool Purchased;
    public bool Applied;
    public float Cost;
    public Button PurchaseButton;
    public Button ApplyButton;
    public ThemeData Data;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PurchaseTheme()
    {
        Purchased = true;
        TM.PurchaseTheme(Cost);
        TM.UpdateThemeUI();
    }
    public void ToggleTheme()
    {
        if (Applied)
        {
            RemoveTheme();
        }
        else
        {
            TM.SetTheme(this);
        }
    }

    public void ApplyTheme()
    {
        Applied = true;
        ThemeOverlay.SetActive(true);
        Background.color = AppliedColour;
    }
    public void RemoveTheme()
    {
        Applied = false;
        ThemeOverlay.SetActive(false);
        Background.color = NormalColour;
    }
    public void UpdateUI(float c)
    {
        if (Purchased)
        {
            PurchaseButton.interactable = false;
            ApplyButton.interactable = true;
            if (Applied)
            {
                Background.color = AppliedColour;
                ThemeOverlay.SetActive(true);
            }
            else
            {
                Background.color = NormalColour;
                ThemeOverlay.SetActive(false);
            }
        }
        else
        {
            if (c > Cost)
            {
                PurchaseButton.interactable = true;
            }
            else
            {
                PurchaseButton.interactable = false;
            }
            ApplyButton.interactable = false;
        }
    }
    public void StoreData()
    {
        var newThemeData = new ThemeData();
        newThemeData.Applied = Applied;
        newThemeData.Purchased = Purchased;
        newThemeData.Name = name;

        Data = newThemeData;
    }
}
