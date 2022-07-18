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


    private void Start()
    {
        NewRequest(1);
    }
    public void NewRequest(float l)
    {
        var level = Random.Range(-2, l+1);
        level = Mathf.RoundToInt(Mathf.Clamp(level, 1, 9999));
        Level = level;
        Reward = (Random.Range(5,11) + Level * 2) * 4;
        var type = Random.Range(0, 6);
        if(type == 0)
        {
            Type = RequestType.Sword;
        }
        else if (type == 1)
        {
            Type = RequestType.Club;
        }
        else if (type == 2)
        {
            Type = RequestType.Bow;
        }
        else if (type == 3)
        {
            Type = RequestType.Wand;
        }
        else if (type == 4)
        {
            Type = RequestType.Helm;
        }
        else if (type == 5)
        {
            Type = RequestType.Armour;
        }
        RequestIcon.sprite = Manager.TypeSprites[type];
        RequestText.text = "Level " + Level.ToString() + " or higher " + Type.ToString() + " (" + Reward + "G)";
        CompleteImage.SetActive(false);

    }
    public void Complete()
    {
        CompleteImage.SetActive(true);
        
    }
}
