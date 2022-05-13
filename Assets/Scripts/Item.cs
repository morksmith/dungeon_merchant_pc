using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public float Level;
    public string ItemName;
    public Image ItemSprite;
    public Image DemandIcon;
    public Sprite DownSprite;
    public Sprite UpSprite;
    public Sprite DoubleUpSprite;
    public float PriceScale = 1;
    public float BasePrice;
    public float Price;
    public ItemType Type;
    public GameObject CollectButton;
    public GameObject PriceInfo;
    public TextMeshProUGUI PriceText;
    public float DamageType;
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
        if (gameObject.GetComponent<Weapon>())
        {
            BasePrice = gameObject.GetComponent<Weapon>().Damage * 8;
        }
        stockMan = GameObject.FindObjectOfType<StockManager>();
        PriceText.text = Price + "G";
        Price = Mathf.CeilToInt(BasePrice * PriceScale);
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

    public void EquipItem()
    {
        PriceInfo.SetActive(false);
        Equipped = true;
    }

    public void StockItem()
    {
        PriceInfo.SetActive(true);
        Equipped = false;
    }

    public void UpdatePrice(float x)
    {
        PriceScale = x;
        if(x == 0.5f)
        {
            DemandIcon.sprite = DownSprite;
            DemandIcon.color = Color.blue;
            DemandIcon.gameObject.SetActive(true);
        }
        else if (x == 1)
        {
            DemandIcon.gameObject.SetActive(false);
        }
        else if (x == 1.5f)
        {
            DemandIcon.sprite = UpSprite;
            DemandIcon.color = Color.yellow;
            DemandIcon.gameObject.SetActive(true);
        }
        else if (x == 2)
        {
            DemandIcon.sprite = DoubleUpSprite;
            DemandIcon.color = Color.yellow;
            DemandIcon.gameObject.SetActive(true);
        }
        Price = Mathf.CeilToInt(BasePrice * PriceScale);
        PriceText.text = Price + "G";
        SellTime = Price;
    }
}
