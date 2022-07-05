using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main Scene");
    }
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial Scene");
    }
    public void LoadStartScene()
    {
        SceneManager.LoadScene("Start");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}

