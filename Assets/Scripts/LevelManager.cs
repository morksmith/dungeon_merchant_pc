using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public SaveManager Save;
    
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main Scene");
        PlayerPrefs.SetInt("Game Started", 1);

    }
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial Scene");
        PlayerPrefs.SetInt("Game Started", 1);

    }
    public void LoadIntro()
    {
        SceneManager.LoadScene("Intro Cutscene");
        PlayerPrefs.SetInt("Game Started", 1);

    }
    public void LoadStartScene()
    {
        if(!Save.InnSaved || !Save.ItemsSaved || !Save.HeroesSaved || !Save.StockSaved || !Save.DungeonSaved || !Save.RequestsSaved)
        {
            Debug.Log("Save not completed!");
            return;
        }
        PlayerPrefs.SetInt("Closed Correctly", 1);
        SceneManager.LoadScene("Start");
        var musicSource = GameObject.FindObjectOfType<MusicManager>().gameObject;
        Destroy(musicSource.gameObject);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}

