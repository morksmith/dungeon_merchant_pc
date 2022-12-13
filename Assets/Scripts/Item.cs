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
    public float SellTimer;
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
            if(SellTimer < SellTime)
            {
                SellTimer += Time.deltaTime * stockMan.SellSpeed;
            }
            else
            {
                ReadyToSell();
            }
            SellSlider.value = SellTimer / SellTime;
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
        if (!stockMan.Tutorial)
        {
            stockMan.Save.SaveGame();
        }
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

    public void ReturnToStock()
    {
        transform.SetParent(stockMan.StockList);
        transform.position = new Vector3(0, 0, 0);
        StockItem();
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
                con.Value = 20 + con.Level * 2;
                BasePrice = con.Value / 2;
                LevelText.text = con.Value.ToString();

            }
            else if (con.Type == Consumable.ConsumableType.Portal)
            {
                BasePrice = 100;
                LevelText.text = "";
            }
            else if (con.Type == Consumable.ConsumableType.Damage)
            {
                con.Value = con.Level + 1;
                BasePrice = 20 * con.Value;
                LevelText.text = con.Value.ToString();
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
        StoreData();
    }

    public void PriceUI()
    {
        PriceText.text = Price + "G";
        if (gameObject.GetComponent<Weapon>())
        {
            LevelText.text = gameObject.GetComponent<Weapon>().Level.ToString();
        }
        else if (gameObject.GetComponent<Armour>())
        {
            LevelText.text = gameObject.GetComponent<Armour>().Level.ToString();
        }
        else if (gameObject.GetComponent<Consumable>())
        {
            var con = gameObject.GetComponent<Consumable>();
            if(con.Type == Consumable.ConsumableType.Potion)
            {
                LevelText.text = con.Value.ToString();
            }
            else if(con.Type == Consumable.ConsumableType.Portal)
            {
                LevelText.text = " ";
            }
            else if (con.Type == Consumable.ConsumableType.Damage)
            {
                LevelText.text = con.Value.ToString();
            }

        }
        
    }

    public void StoreData()
    {
        var newItemData = new ItemData();
        newItemData.ItemName = ItemName;
        newItemData.SpriteIndex = SpriteIndex;
        newItemData.TypeIndex = TypeIndex;
        newItemData.DamageType = DamageType;
        newItemData.Merchant = Merchant;
        newItemData.Equipped = Equipped;
        newItemData.Selling = Selling;
        newItemData.Sold = Sold;
        newItemData.SellTimer = SellTimer;
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
            else if (GetComponent<Consumable>().Type == Consumable.ConsumableType.Portal)
            {
                newItemData.ConsumableType = 2;
            }
            else if (GetComponent<Consumable>().Type == Consumable.ConsumableType.Damage)
            {
                newItemData.ConsumableType = 3;
            }
        }
        newItemData.BasePrice = BasePrice;
        if (Selling)
        {
            newItemData.BasePrice = Price;
        }
        Data = newItemData;
    }

    
}
