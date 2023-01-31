using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicChest : MonoBehaviour
{
    public StockManager Stock;
    public Slider TimeSlider;
    public float RestockTime = 300;
    public float Timer;
    public bool Restocking = false;
    public bool ReadyToCollect = false;
    public Button MagicChestButton;
    public Image MagicChestSprite;
    public InnManager Inn;
    public SaveManager Save;
    public GameObject NewIcon;
    public MagicChestData Data;
    

    private void Start()
    {
        RestockTime = 300;
        if (PlayerPrefs.GetInt("MagicChestUnlocked") == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        if (ReadyToCollect)
        {
            Restock();
        }
        CheckTimePassed();
    }

    private void Update()
    {
        if (Restocking)
        {
            if(Timer < RestockTime)
            {
                Timer += Time.deltaTime;
                TimeSlider.value = Timer / RestockTime;
            }
            else
            {
                Restock();
            }
            
        }
    }

    public void CheckTimePassed()
    {
        if (Stock.Tutorial)
        {
            return;
        }
        var lastDateChecked = System.DateTime.Parse(PlayerPrefs.GetString("DateTime"));
        var elapsedSeconds = (int)System.DateTime.Now.Subtract(lastDateChecked).TotalSeconds;
        Timer += elapsedSeconds;
        if (Timer > RestockTime)
        {
            Restock();
        }
        if (Restocking)
        {
            NewIcon.SetActive(false);
            MagicChestButton.interactable = false;
            MagicChestSprite.color = new Color(1, 1, 1, 0.5f);
            TimeSlider.gameObject.SetActive(true);
        }

    }

    public void Restock()
    {
        Stock.PlayLevelUpSound();
        Restocking = false;
        ReadyToCollect = true;
        NewIcon.SetActive(true);
        MagicChestButton.interactable = true;
        MagicChestSprite.color = new Color(1, 1, 1, 1);
        TimeSlider.gameObject.SetActive(false);
        Inn.BottomContent.NewSaleIcon();
        
    }

    public void CollectChest()
    {
        var merchLevel = (Stock.MaxProfit / 1000);
        merchLevel = Mathf.CeilToInt(merchLevel);
        Stock.AddChest(4, (int)Random.Range(1, merchLevel));
        Restocking = true;
        Timer = 0;
        ReadyToCollect = false;
        NewIcon.SetActive(false);
        MagicChestButton.interactable = false;
        MagicChestSprite.color = new Color(1, 1, 1, 0.5f);
        TimeSlider.gameObject.SetActive(true);
        Inn.BottomContent.NewItemIcon();
        Save.SaveGame();
    }

    public void StoreData()
    {
        var newMagicChestData = new MagicChestData();
        newMagicChestData.LastDateChecked = System.DateTime.Now;
        newMagicChestData.Timer = Timer;
        newMagicChestData.RestockTime = RestockTime;
        newMagicChestData.Restocking = Restocking;
        newMagicChestData.ReadyToCollect = ReadyToCollect;
        Data = newMagicChestData;
    }


}
