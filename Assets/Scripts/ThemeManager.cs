using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public List<ShopTheme> Themes;
    public StockManager Stock;


    public void SetTheme(ShopTheme t)
    {
        foreach (ShopTheme st in Themes)
        {
            st.RemoveTheme();
        }
        t.ApplyTheme();
        UpdateThemeUI();
        Stock.Save.SaveGame();
    }
    public void UpdateThemeUI()
    {
        foreach(ShopTheme t in Themes)
        {
            t.UpdateUI(Stock.Gold);
        }
    }
    public void PurchaseTheme(float c)
    {
        Stock.CollectGold(-c);
    }
}
