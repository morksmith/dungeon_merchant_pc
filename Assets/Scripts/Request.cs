using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Request : MonoBehaviour
{
    public float Level;
    public float Reward;
    public Image RequestIcon;
    public int TypeIndex;
    public enum RequestType
    {
        Sword,
        Club,
        Bow,
        Wand,
        Helm,
        Armour
    }
    public RequestType Type;
    public TextMeshProUGUI RequestText;
    public GameObject CompleteImage;
    public RequestManager Manager;
    public RequestData Data;
    public bool TaskComplete = false;

    private void Start()
    {
        
    }
    public void NewRequest(float l)
    {
        var level = Random.Range(-2, l+1);
        level = Mathf.RoundToInt(Mathf.Clamp(level, 1, 9999));
        Level = level;
        Reward = (Random.Range(5,11) + Level * 2) * 4;
        TypeIndex = Random.Range(0, 6);
        if(TypeIndex == 0)
        {
            Type = RequestType.Sword;
        }
        else if (TypeIndex == 1)
        {
            Type = RequestType.Club;
        }
        else if (TypeIndex == 2)
        {
            Type = RequestType.Bow;
        }
        else if (TypeIndex == 3)
        {
            Type = RequestType.Wand;
        }
        else if (TypeIndex == 4)
        {
            Type = RequestType.Helm;
        }
        else if (TypeIndex == 5)
        {
            Type = RequestType.Armour;
        }
        RequestIcon.sprite = Manager.TypeSprites[TypeIndex];
        RequestText.text = "Level " + Level.ToString() + " or higher " + Type.ToString() + " (" + Reward + "G)";
        CompleteImage.SetActive(false);
        TaskComplete = false;

        StoreData();

    }
    public void Complete()
    {
        CompleteImage.SetActive(true);
        TaskComplete = true;
        StoreData();
    }

    public void SpecificRequest(RequestData rd)
    {
        Level = rd.Level;
        Reward = rd.Reward;
        TypeIndex = rd.RequestType;
        if (TypeIndex == 0)
        {
            Type = RequestType.Sword;
        }
        else if (TypeIndex == 1)
        {
            Type = RequestType.Club;
        }
        else if (TypeIndex == 2)
        {
            Type = RequestType.Bow;
        }
        else if (TypeIndex == 3)
        {
            Type = RequestType.Wand;
        }
        else if (TypeIndex == 4)
        {
            Type = RequestType.Helm;
        }
        else if (TypeIndex == 5)
        {
            Type = RequestType.Armour;
        }
        RequestIcon.sprite = Manager.TypeSprites[TypeIndex];
        RequestText.text = "Level " + Level.ToString() + " or higher " + Type.ToString() + " (" + Reward + "G)";
        if (rd.Complete)
        {
            Complete();
        }
        else
        {
            CompleteImage.SetActive(false);
            TaskComplete = false;
        }       

        StoreData();
    }

    public void StoreData()
    {
        var newData = new RequestData();
        newData.Level = Level;
        newData.Reward = Reward;
        newData.RequestType = TypeIndex;
        newData.Complete = TaskComplete;

        Data = newData;

    }
}
