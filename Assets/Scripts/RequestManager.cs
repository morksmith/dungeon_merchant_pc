using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    public StockManager Stock;
    public List<Request> CurrentRequests;
    public List<Sprite> TypeSprites;

    public void RequestCompleted(Request r)
    {
        Stock.CollectGold(r.Reward);
        
    }

    public void CompleteRequest(Request r)
    {
        Destroy(Stock.DraggedItem.gameObject);
        Stock.DraggedItem = null;
        Stock.CollectGold(r.Reward);
        r.Complete();
    }

    public void NewRequests()
    {
        for (var r = 0; r < CurrentRequests.Count; r++)
        {
            CurrentRequests[r].NewRequest(Mathf.CeilToInt(Stock.MaxProfit / 100));
        }
    }

}
