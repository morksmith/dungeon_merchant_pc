using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    public string PlayerPrefString;
    void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefString) > 0)
        {
            gameObject.SetActive(false);
        }
    }

    
}
