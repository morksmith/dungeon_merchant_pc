using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveManager : MonoBehaviour
{
    public LevelManager Level;
    public Button ContinueButton;

    private void Start()
    {
        if(PlayerPrefs.GetFloat("Game Started") == 0)
        {
            PlayerPrefs.SetFloat("Music Volume", 1);
            PlayerPrefs.SetFloat("SFX Volume", 1);
            PlayerPrefs.SetFloat("Game Started", 1);
        }

        if(PlayerPrefs.GetFloat("New Game") == 0)
        {
            if (ContinueButton != null)
            {
                ContinueButton.interactable = false;
            }
        }
        else
        {
            if (ContinueButton != null)
            {
                ContinueButton.interactable = true;
            }
        }

    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Music Volume", 1);
        PlayerPrefs.SetFloat("SFX Volume", 1);

    }

    public void StartNewGame()
    {
        PlayerPrefs.SetFloat("New Game", 1);

    }


}
