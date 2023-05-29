using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public List<ShopTheme> Themes;
    public StockManager Stock;
    public ShopTheme[] AllThemes;
    private float completeThemes;


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
        completeThemes = 0;
        AllThemes = null;
        AllThemes = GameObject.FindObjectsOfType<ShopTheme>();
        Debug.Log(AllThemes.Length);
        for(var i = 0; i < AllThemes.Length; i++)
        {
            if (AllThemes[i].Purchased)
            {
                completeThemes++;
            }
        }
        if(completeThemes >= AllThemes.Length)
        {
            var ach = new Steamworks.Data.Achievement("decked_out");
            ach.Trigger();
        }
        completeThemes = 0;
    }
}
