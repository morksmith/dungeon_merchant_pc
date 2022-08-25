using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public string ItemName;    
    public Image ItemSprite;
    public int SpriteIndex;
    public Image DemandIcon;
    public Sprite DownSprite;
    public Sprite UpSprite;
    public Sprite DoubleUpSprite;
    public float PriceScale = 1;
    public float BasePrice;
    public float Price;
    public ItemType Type;
    public int TypeIndex;
    public GameObject CollectButton;
    public GameObject PriceInfo;
    public TextMeshProUGUI PriceText;
    public float DamageType;
    public float SellTime;
    public bool Locked;
    public bool Equipped;
    public bool Selling;
    public bool Sold;
    public bool Merchant;
    public Slider SellSlider;
    public TextMeshProUGUI LevelText;
    public ItemData Data;

    private StockManager stockMan;
    private MerchantMenu merch;
    private float sellTimer;
    // Start is called before the first frame update
    private void Start()
    {
        if(stockMan == null)
        {
            stockMan = GameObject.FindObjectOfType<StockManager>();
        }
        if(merch == null)
        {
            merch = GameObject.FindObjectOfType<MerchantMenu>();
        }

        StoreData();



        
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
                sellTimer += Time.deltaTime * stockMan.SellSpeed;
            }
            else
            {
                ReadyToSell();
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
        if (Merchant)
        {
            merch.SelectItem(this);
        }
        else
        {
            stockMan.SelectItem(this);
        }

    }
    public void CollectGold()
    {
        stockMan.CollectGold(Price);
        stockMan.PlayCollectSound();
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
        if (Merchant)
        {
            if(stockMan!= null)
            {
                Price *= stockMan.MerchantDiscount;
            }
            Price = Mathf.CeilToInt(Price);
        }
        PriceText.text = Price + "G";        
        SellTime = Price;
    }

    public void SetStats()
    {
        if (gameObject.GetComponent<Weapon>())
        {
            gameObject.GetComponent<Weapon>().Damage += (gameObject.GetComponent<Weapon>().Level * 2);
            BasePrice = gameObject.GetComponent<Weapon>().Damage * 3;
            LevelText.text = gameObject.GetComponent<Weapon>().Level.ToString();
        }
        else if (gameObject.GetComponent<Armour>())
        {
            gameObject.GetComponent<Armour>().HP += (gameObject.GetComponent<Armour>().Level * 2);
            BasePrice = gameObject.GetComponent<Armour>().HP * 3;
            LevelText.text = gameObject.GetComponent<Armour>().Level.ToString();
        }
        else if (gameObject.GetComponent<Consumable>())
        {
            var con = gameObject.GetComponent<Consumable>();
            if (con.Type == Consumable.ConsumableType.Potion)
            {
                con.Value = con.Level * 20;
                BasePrice = con.Value / 2;
                LevelText.text = con.Value.ToString();

            }
            else if (con.Type == Consumable.ConsumableType.Portal)
            {
                BasePrice = 100;
                LevelText.text = "";
            }
        }
        else
        {
            LevelText.text = " ";
        }
        stockMan = GameObject.FindObjectOfType<StockManager>();
        merch = GameObject.FindObjectOfType<MerchantMenu>();
        Price = Mathf.CeilToInt(BasePrice * PriceScale);
        PriceText.text = Price + "G";
        SellTime = Price;
        stockMan.UpdatePrices();
    
}
    public void ReadyToSell()
    {
        Selling = false;
        Sold = true;
        CollectButton.SetActive(true);
        CollectButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD\n" + Price + "G";
        stockMan.NewSale();
    }

    public void StoreData()
    {
        var newItemData = new ItemData();
        newItemData.ItemName = ItemName;
        newItemData.SpriteIndex = SpriteIndex;
        newItemData.BasePrice = BasePrice;
        newItemData.TypeIndex = TypeIndex;
        newItemData.DamageType = DamageType;
        newItemData.Merchant = Merchant;
        newItemData.Equipped = Equipped;
        newItemData.Selling = Selling;
        if(GetComponent<Weapon>() != null)
        {
            newItemData.ItemName = GetComponent<Weapon>().WeaponName;
            newItemData.Level = GetComponent<Weapon>().Level;
            newItemData.StatPoint = GetComponent<Weapon>().Damage;
            newItemData.ConsumableType = 0;
        }
        else if(GetComponent<Armour>() != null)
        {
            newItemData.Level = GetComponent<Armour>().Level;
            newItemData.StatPoint = GetComponent<Armour>().HP;
            newItemData.ConsumableType = 0;
        }
        else if (GetComponent<Consumable>() != null)
        {
            newItemData.Level = GetComponent<Consumable>().Level;
            newItemData.StatPoint = GetComponent<Consumable>().Value;
            if(GetComponent<Consumable>().Type == Consumable.ConsumableType.Potion)
            {
                newItemData.ConsumableType = 1;
            }
            else
            {
                newItemData.ConsumableType = 2;
            }
        }
        Data = newItemData;
    }

    
}
