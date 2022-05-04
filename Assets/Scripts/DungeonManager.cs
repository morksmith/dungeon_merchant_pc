using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DungeonManager : MonoBehaviour
{
    public float CurrentTime;
    public float Minutes;
    public float Hours;
    public float MaxTime = 1440;
    public Slider TimeSlider;
    public TextMeshProUGUI TimeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentTime < MaxTime)
        {
            CurrentTime += Time.deltaTime * 4;
        }
        else
        {
            CurrentTime = 0;
        }
        
        Hours = (CurrentTime / 60) % 24;
        Minutes = (CurrentTime  % 60);

        Hours = Mathf.FloorToInt(Hours);
        Minutes = Mathf.FloorToInt(Minutes);

        string hourDisplay = Hours.ToString("00");
        string minuteDisplay = Minutes.ToString("00");

        TimeText.text = hourDisplay + ":" + minuteDisplay;
        TimeSlider.value = CurrentTime / MaxTime;



    }
}
