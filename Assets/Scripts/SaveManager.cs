using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public LevelManager Level;

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Level.LoadMainScene();
    }
}
