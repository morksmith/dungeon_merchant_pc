using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blocker : MonoBehaviour
{
    public string PlayerPrefString;
    public Button SurvivalButton;
    void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefString) > 0)
        {
            gameObject.SetActive(false);
            SurvivalButton.interactable = true;
        }
    }

    
}
