using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public string ItemName;
    public Image ItemSprite;
    public float Price;
    public ItemType Type;
    public GameObject CollectButton;
    public float SellTime;
    public bool Locked;
    public bool Equipped;
    public bool Selling;
    public bool Sold;
    public Slider SellSlider;

    private StockManager stockMan;
    private float sellTimer;
    // Start is called before the first frame update
    void Start()
    {
        stockMan = GameObject.FindObjectOfType<StockManager>();
       
        SellTime = Price;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Selling)
        {
            SellSlider.gameObject.SetActive(true);
            GetComponent<Button>().interactable = false;
            if(sellTimer < SellTime)
            {
                sellTimer += Time.deltaTime;
            }
            else
            {
                Selling = false;
                Sold = true;
                CollectButton.SetActive(true);
                CollectButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD\n" + Price + "G";
            }
            SellSlider.value = sellTimer / SellTime;
        }
        else
        {
            SellSlider.gameObject.SetActive(false);
        }
    }

    public void SelectItem()
    {
        stockMan.SelectItem(this);
    }
    public void CollectGold()
    {
        stockMan.CollectGold(Price);
        Destroy(gameObject);
    }
}
