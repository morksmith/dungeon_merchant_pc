using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prospector : MonoBehaviour
{
    public Menu ProspectorMenu;
    public Button ProspectorButton;
    public Image ProspectorSprite;
    public GameObject NewIcon;
    public Slider TimeSlider;
    public float ProspectTime = 300;
    public float Timer;
    public bool NewProspector;
    public bool Mining = false;
    public bool ReturnedFromMining = false;
    public GameObject ClaimGoldButton;
    public InnManager Inn;
    public ProspectorData Data;


    private void Start()
    {
        ProspectorButton = gameObject.GetComponent<Button>();
        if (PlayerPrefs.GetInt("ProspectorUnlocked") == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        CheckTimePassed();

    }

    public void Update()
    {
        if (Mining)
        {
            Timer += Time.deltaTime;
            TimeSlider.value = Timer / ProspectTime;
            if(Timer > ProspectTime)
            {
                ReturnFromMining();
            }
        }

    }

    public void CheckTimePassed()
    {
        var lastDateChecked = System.DateTime.Parse(PlayerPrefs.GetString("DateTime"));
        var elapsedSeconds = (int)System.DateTime.Now.Subtract(lastDateChecked).TotalSeconds;
        Debug.Log("Last Date: " + lastDateChecked + "\nCurrent Date: " + System.DateTime.Now + "\nSeconds Passed = " + elapsedSeconds);
        Timer += elapsedSeconds;
        if(Timer > ProspectTime)
        {
            ReturnFromMining();
        }
        if (Mining)
        {
            NewIcon.SetActive(false);
            ProspectorButton.interactable = false;
            ProspectorSprite.color = new Color(1, 1, 1, 0.5f);
            TimeSlider.gameObject.SetActive(true);
        }
        
    }
    public void ReturnFromMining()
    {
        Mining = false;
        Timer = 0;
        ReturnedFromMining = true;
        NewIcon.SetActive(true);
        ProspectorButton.interactable = true;
        ProspectorSprite.color = new Color(1, 1, 1, 1);
        TimeSlider.gameObject.SetActive(false);
        Inn.BottomContent.NewHeroIcon();
    }

    public void SendToMine()
    {
        Mining = true;
        Timer = 0;
        ReturnedFromMining = false;
        NewIcon.SetActive(false);
        ProspectorButton.interactable = false;
        ProspectorSprite.color = new Color(1, 1, 1, 0.5f);
        TimeSlider.gameObject.SetActive(true);

    }

    public void StoreData()
    {
        var newProspectorData = new ProspectorData();
        newProspectorData.LastDateChecked = System.DateTime.Now;
        newProspectorData.Timer = Timer;
        newProspectorData.Mining = Mining;
        newProspectorData.ReturnedFromMining = ReturnedFromMining;
        Data = newProspectorData;
    }
}
