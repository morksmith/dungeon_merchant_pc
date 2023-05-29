using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    public StockManager Stock;
    public List<Request> CurrentRequests;
    public List<Sprite> TypeSprites;
    public Request[] AllRequests;
    private float completedTasks;

    public void RequestCompleted(Request r)
    {
        Stock.CollectGold(r.Reward);
        
    }

    public void CompleteRequest(Request r)
    {
        Stock.PlayCollectSound();
        Destroy(Stock.DraggedItem.gameObject);
        Stock.DraggedItem = null;
        Stock.CollectGold(r.Reward);
        r.Complete();
        Stock.DeselectItems();
        AllRequests = null;
        AllRequests = GameObject.FindObjectsOfType<Request>();
        completedTasks = 0;
        for(var i = 0; i < AllRequests.Length; i++)
        {
            if (AllRequests[i].TaskComplete)
            {
                completedTasks++;
            }
        }
        if(completedTasks >= 4)
        {
            var ach = new Steamworks.Data.Achievement("happy_customer");
            ach.Trigger();
        }
        completedTasks = 0;
    }

    public void NewRequests()
    {
        for (var r = 0; r < CurrentRequests.Count; r++)
        {
            CurrentRequests[r].NewRequest(Mathf.CeilToInt((Stock.MaxProfit / 1000)), Stock.RequestBonus);
        }
    }

    public void CreateSpecificRequests(RequestData[] rd)
    {
        for (var r = 0; r < CurrentRequests.Count; r++)
        {
            CurrentRequests[r].SpecificRequest(rd[r]);
        }
    }

}
