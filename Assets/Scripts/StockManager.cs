using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StockManager : MonoBehaviour
{
    public float Gold;
    public float MaxProfit;
    public ItemGenerator Generator;
    public Transform ItemBox;
    public Item[] AllItems;
    public Item CurrentItem;
    public Item DraggedItem;
    public TextMeshProUGUI ItemInfoText;
    public TextMeshProUGUI GoldText;
    public Transform StockList;
    public Transform ShopList;
    public GameObject StockDropZone;
    public GameObject ChestPrefab;
    public GameObject WeaponPrefab;
    public GameObject ArmourPrefab;
    public GameObject ConsumablePrefab;
    public HeroManager Hero;
    public List<Item> ItemList;
    public float SwordPrice = 1.5f;
    public float ClubPrice = 1;
    public float BowPrice = 1;
    public float WandPrice = 1;
    public float ArmourPrice = 1;
    public float PotionPrice = 1;
    public int ShopSlots = 4;
    // Start is called before the first frame update
    void Start()
    {
        GoldText.text = Gold + "G";
        UpdatePrices();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                    eventDataCurrentPosition.position = touch.position;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                    if (results.Count > 0)
                    {
                        if (results[0].gameObject.GetComponent<Item>() != null && !results[0].gameObject.GetComponent<Item>().Merchant && !results[0].gameObject.GetComponent<Item>().Selling)
                        {
                            DraggedItem = results[0].gameObject.GetComponent<Item>();
                            SelectItem(results[0].gameObject.GetComponent<Item>());
                        }
                        else
                        {
                            DraggedItem = null;
                            StockDropZone.SetActive(false);
                            ItemBox.gameObject.SetActive(false);
                            break;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    
                    

                    
                    if (DraggedItem != null)
                    {
                        StockDropZone.SetActive(true);
                        ItemBox.gameObject.SetActive(true);
                        ItemBox.position = touch.position;
                        ItemBox.GetComponent<Image>().sprite = DraggedItem.ItemSprite.sprite;
                        break;
                    }
                    break;

                case TouchPhase.Ended:
                    if(DraggedItem == null)
                    {
                        break;
                    }
                    eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                    eventDataCurrentPosition.position = touch.position;
                    results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                    if (results.Count > 0)
                    {

                        if (results[0].gameObject.GetComponent<EquipmentSlot>() != null)
                        {
                            if (results[0].gameObject.GetComponent<EquipmentSlot>().Type.Type == DraggedItem.Type.Type)
                            {
                                if (results[0].gameObject.GetComponent<EquipmentSlot>().WeaponSlot)
                                {
                                    if (DraggedItem.GetComponent<Weapon>()!= null)
                                    {
                                        if(DraggedItem.DamageType == results[0].gameObject.GetComponent<EquipmentSlot>().DamageType)
                                        {
                                            Debug.Log("Dropped on Equipment Slot");
                                            DraggedItem.gameObject.transform.SetParent(results[0].gameObject.transform);
                                            DraggedItem.transform.position = results[0].gameObject.transform.position;
                                            DraggedItem.EquipItem();
                                            Hero.SelectedHero.EquipWeapon(DraggedItem.GetComponent<Weapon>());
                                            Hero.OpenEquipMenu();
                                            Hero.SelectedHero.SelectHero();
                                            Hero.SelectHero(Hero.SelectedHero);
                                            CurrentItem = null;
                                            ItemInfoText.text = "SELECT AN ITEM";
                                            ItemInfoText.color = Color.white;

                                        }
                                        else
                                        {
                                            Debug.Log("Wrong Weapon Type");
                                        }
                                    }
                                }
                                else
                                {
                                    DraggedItem.gameObject.transform.SetParent(results[0].gameObject.transform);
                                    DraggedItem.transform.position = results[0].gameObject.transform.position;
                                    DraggedItem.EquipItem();
                                    if(DraggedItem.GetComponent<Weapon>()!= null)
                                    {
                                        Hero.SelectedHero.EquipWeapon(DraggedItem.GetComponent<Weapon>());
                                        Debug.Log("Equipped Weapon");
                                    }
                                    if (DraggedItem.GetComponent<Armour>() != null)
                                    {
                                        Hero.SelectedHero.EquipArmour(DraggedItem.GetComponent<Armour>());
                                        Debug.Log("Equipped Armour");
                                    }
                                    Hero.OpenEquipMenu();
                                    Hero.SelectedHero.SelectHero();
                                    Hero.SelectHero(Hero.SelectedHero);
                                    CurrentItem = null;
                                    ItemInfoText.text = "SELECT AN ITEM";
                                    ItemInfoText.color = Color.white;


                                }

                            }
                            else
                            {
                                Debug.Log("Wrong Item Type");
                            }
                        }
                        if (results[0].gameObject.GetComponent<DropZone>() != null)
                        {
                            if (DraggedItem.Equipped)
                            {
                                if (DraggedItem.GetComponent<Weapon>())
                                {
                                    Hero.SelectedHero.UnequipWeapon(DraggedItem.GetComponent<Weapon>());
                                }
                                if (DraggedItem.GetComponent<Armour>())
                                {
                                    Hero.SelectedHero.UneQuipArmour(DraggedItem.GetComponent<Armour>());
                                }
                                Hero.SelectHero(Hero.SelectedHero);
                                Hero.OpenEquipMenu();
                            }
                            Debug.Log("Item Returned to Stock");
                            DraggedItem.gameObject.transform.SetParent(StockList);
                            DraggedItem.transform.position = new Vector3(0, 0, 0);
                            DraggedItem.StockItem();
                            if(Hero.SelectedHero != null)
                            {
                                Hero.SelectedHero.SelectHero();
                            }
                            UpdatePrices();
                        }
                        if (results[0].gameObject.GetComponent<Item>() != null)
                        {
                            if((results[0].gameObject.GetComponent<Item>().Equipped && results[0].gameObject.GetComponent<Item>().Type.Type == DraggedItem.Type.Type) && results[0].gameObject.GetComponent<Item>() != DraggedItem)
                            {
                                
                                if (DraggedItem.GetComponent<Weapon>())
                                {
                                    if (DraggedItem.DamageType == results[0].gameObject.GetComponent<Item>().DamageType)
                                    {
                                        Debug.Log("Replaced Item");
                                        DraggedItem.gameObject.transform.SetParent(results[0].gameObject.transform.parent);
                                        DraggedItem.transform.position = results[0].gameObject.transform.parent.position;
                                        DraggedItem.EquipItem();
                                        results[0].gameObject.GetComponent<Item>().StockItem();
                                        results[0].gameObject.GetComponent<Item>().transform.SetParent(StockList);
                                        Hero.SelectedHero.UnequipWeapon(results[0].gameObject.GetComponent<Weapon>());
                                        Hero.SelectedHero.EquipWeapon(DraggedItem.GetComponent<Weapon>());
                                        Hero.OpenEquipMenu();
                                        Hero.SelectedHero.SelectHero();
                                        Hero.SelectHero(Hero.SelectedHero);
                                        CurrentItem = null;
                                        ItemInfoText.text = "SELECT AN ITEM";
                                        UpdatePrices();
                                    }
                                    else
                                    {
                                        Debug.Log("Wrong Weapon Type");
                                    }
                                }
                                else if (DraggedItem.GetComponent<Armour>())
                                {
                                    Debug.Log("Replaced Item");
                                    DraggedItem.gameObject.transform.SetParent(results[0].gameObject.transform.parent);
                                    DraggedItem.transform.position = results[0].gameObject.transform.parent.position;
                                    DraggedItem.EquipItem();
                                    results[0].gameObject.GetComponent<Item>().StockItem();
                                    results[0].gameObject.GetComponent<Item>().transform.SetParent(StockList);
                                    Hero.SelectedHero.UneQuipArmour(results[0].gameObject.GetComponent<Armour>());
                                    Hero.SelectedHero.EquipArmour(DraggedItem.GetComponent<Armour>());
                                    Hero.OpenEquipMenu();
                                    Hero.SelectedHero.SelectHero();
                                    Hero.SelectHero(Hero.SelectedHero);
                                    CurrentItem = null;
                                    ItemInfoText.text = "SELECT AN ITEM";
                                    UpdatePrices();
                                }

                            }                  

                        }

                    }
                    DraggedItem = null;
                    ItemBox.gameObject.SetActive(false);
                    StockDropZone.SetActive(false);

                    break;
            }
        }
    }

    public void SellItem()
    {
        if(CurrentItem != null)
        {
            if(ShopList.childCount < ShopSlots)
            {
                
                if (!CurrentItem.Equipped)
                {
                    CurrentItem.transform.SetParent(ShopList);
                    CurrentItem.Selling = true;
                    CurrentItem = null;
                    ItemInfoText.text = "SELECT AN ITEM";
                    ItemInfoText.color = Color.white;
                }
                else
                {
                    Debug.Log("Cannot sell equipped item");
                    CurrentItem = null;
                    ItemInfoText.text = "CANNOT SELL EQUIPPED ITEM";
                    ItemInfoText.color = Color.white;
                }
            }
            else
            {
                Debug.Log("No available slots in shop");
                ItemInfoText.color = Color.white;
            }


        }
        else
        {
            Debug.Log("No Item selected");
            CurrentItem = null;
            ItemInfoText.text = "SELECT AN ITEM";
            ItemInfoText.color = Color.white;
        }

       
    }

    public void SelectItem(Item i)
    {
        CurrentItem = i;
        if (i.GetComponent<Weapon>() != null)
        {
            {
                ItemInfoText.text = i.GetComponent<Weapon>().WeaponName + " (" + i.GetComponent<Weapon>().Level + ")" + "\nDMG: " + i.GetComponent<Weapon>().Damage + "\n" + i.Price + "G";
                

            }
        }
        else if (i.GetComponent<Armour>() != null)
        {
            {
                ItemInfoText.text = i.ItemName + " (" + i.GetComponent<Armour>().Level + ")" + "\n+HP: " + i.GetComponent<Armour>().HP + "\n" + i.Price + "G";


            }
        }


    }

    public void DeselectItems()
    {
        CurrentItem = null;
        ItemInfoText.text = "SELECT AN ITEM";
    }

    public void CollectGold(float i)
    {
        Gold += i;
        GoldText.text = Gold + "G";
        if(Gold > MaxProfit)
        {
            MaxProfit = Gold;
        }
    }

    public void CalculateArmourAndPotions(float a, float p)
    {
        if(a == 0)
        {
            ArmourPrice = 0.5f;
        }
        if (a == 1)
        {
            ArmourPrice = 1;
        }
        if(a == 2)
        {
            ArmourPrice = 1.5f;
        }
        if(a == 3)
        {
            ArmourPrice = 2;
        }

        if (p == 0)
        {
            PotionPrice = 0.5f;
        }
        if (p == 1)
        {
            PotionPrice = 1;
        }
        if (p == 2)
        {
            PotionPrice = 1.5f;
        }
        if (p == 3)
        {
            PotionPrice = 2;
        }

        UpdatePrices();
    }

    public void CalculateWeaponPrices(float b, float g, float d, float s)
    {
        if(b == 0)
        {
            BowPrice = 0.5f;
        }
        if(b == 1)
        {
            BowPrice = 1;
        }
        if(b == 2)
        {
            BowPrice = 1.5f;
        }
        if(b == 3)
        {
            BowPrice = 2;
        }

        if (g == 0)
        {
            WandPrice = 0.5f;
        }
        if (g == 1)
        {
            WandPrice = 1;
        }
        if (g == 2)
        {
            WandPrice = 1.5f;
        }
        if (g == 3)
        {
            WandPrice = 2;
        }


        if (d == 0)
        {
            SwordPrice = 0.5f;
        }
        if (d == 1)
        {
            SwordPrice = 1;
        }
        if (d == 2)
        {
            SwordPrice = 1.5f;
        }
        if (d == 3)
        {
            SwordPrice = 2;
        }

        if (s == 0)
        {
            ClubPrice = 0.5f;
        }
        if (s == 1)
        {
            ClubPrice = 1;
        }
        if (s == 2)
        {
            ClubPrice = 1.5f;
        }
        if (s == 3)
        {
            ClubPrice = 2;
        }
        UpdatePrices();

    }

    public void UpdatePrices()
    {
        AllItems = GameObject.FindObjectsOfType<Item>();
        for(var i = 0; i < AllItems.Length; i++)
        {
            if (!AllItems[i].Selling)
            {
                if (AllItems[i].gameObject.GetComponent<Weapon>())
                {
                    if (AllItems[i].DamageType == 0)
                    {
                        AllItems[i].UpdatePrice(SwordPrice);
                    }
                    else if (AllItems[i].DamageType == 1)
                    {
                        AllItems[i].UpdatePrice(WandPrice);
                    }
                    else if (AllItems[i].DamageType == 2)
                    {
                        AllItems[i].UpdatePrice(BowPrice);
                    }
                    else if (AllItems[i].DamageType == 3)
                    {
                        AllItems[i].UpdatePrice(ClubPrice);
                    }
                }
            }
            
            
        }
        
    }

    public void AddChest(int d, int l)
    {
        var newChest = Instantiate(ChestPrefab, StockList);
        var c = newChest.GetComponent<Chest>();
        var t = Random.Range(0, 5);
        if (t < 3)
        {
            if (d == 0)
            {
                c.Type = Chest.ChestType.Armour;
            }
            else if (d == 1)
            {
                c.Type = Chest.ChestType.Consumable;
            }
            else if (d == 2)
            {
                c.PickRandom();
            }
            else if (d == 3)
            {
                c.Type = Chest.ChestType.Weapon;
            }
        }
        c.Level = l;
    }

    public void CreateItem(int t, int l)
    {
        if(t == 0)
        {
            Generator.GenerateArmour(l);
        }
        else if (t == 1)
        {
            Generator.GenerateWeapon(l, false);
        }
        else if (t == 3)
        {
            Generator.GenerateConsumable();
        }

    }

    
}
