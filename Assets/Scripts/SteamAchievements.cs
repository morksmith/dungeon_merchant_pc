using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering;

public class SteamAchievements : MonoBehaviour
{
    

    public void Start()
    {
        
    }

    public void CheckAchievment(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        Debug.Log($"Achievment {id} status: " + ach.State);
    }

    public void UnlockAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        Debug.Log($"Achievment {id} unlocked");
    }

    public void ClearAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
        Debug.Log($"Achievment {id} cleared");
    }

    public void AddToStat(string id)
    {
        var stat = new Steamworks.Data.Stat(id);
        stat.Add(1);
    }

    public void CompleteGame()
    {
        var ach = new Steamworks.Data.Achievement("game_complete");
        ach.Trigger();
    }

}
