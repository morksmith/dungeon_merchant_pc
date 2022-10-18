using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreText : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    
    void Start()
    {
        ScoreText.text = "HIGH SCORE:\n" + PlayerPrefs.GetFloat("HS");
    }

   
}
