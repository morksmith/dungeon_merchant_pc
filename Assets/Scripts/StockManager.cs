using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StockManager : MonoBehaviour
{
    public float Gold;
    public Transform ItemBox;
    public Item CurrentItem;
    public Item DraggedItem;
    public TextMeshProUGUI ItemInfoText;
    public TextMeshProUGUI GoldText;
    public Transform StockList;
    public Transform ShopList;
    public GameObject StockDropZone;
    public HeroManager Hero;
    public List<Item> ItemList;
    public float SwordPrice = 1;
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
                    
                    break;

                case TouchPhase.Moved:
                    if(DraggedItem != null)
                    {
                        StockDropZone.SetActive(true);
                        ItemBox.gameObject.SetActive(true);
                        ItemBox.position = touch.position;
                        ItemBox.GetComponent<Image>().sprite = DraggedItem.ItemSprite.sprite;
                        break;
                    }

                    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                    eventDataCurrentPosition.position = touch.position;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                    if (results.Count > 0)
                    {
                        if (results[0].gameObject.GetComponent<Item>() != null)
                        {
                            DraggedItem = results[0].gameObject.GetComponent<Item>();
                        }
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
                            Debug.Log("dropped on" + results[0].gameObject.name);
                            if (results[0].gameObject.GetComponent<EquipmentSlot>().Type.Type == DraggedItem.Type.Type)
                            {
                                Debug.Log("Dropped on Equipment Slot");
                                DraggedItem.gameObject.transform.SetParent(results[0].gameObject.transform);
                                DraggedItem.transform.position = results[0].gameObject.transform.position;
                                DraggedItem.Equipped = true;
                                Hero.SelectedHero.EquipWeapon(DraggedItem.GetComponent<Weapon>());
                                Hero.OpenEquipMenu();
                            }
                        }
                        if (results[0].gameObject.GetComponent<DropZone>() != null)
                        {
                            if (DraggedItem.Equipped)
                            {
                                Hero.SelectedHero.UnequipWeapon(DraggedItem.GetComponent<Weapon>());
                                Hero.OpenEquipMenu();
                            }
                            Debug.Log("Item Returned to Stock");
                            DraggedItem.gameObject.transform.SetParent(StockList);
                            DraggedItem.transform.position = new Vector3(0, 0, 0);
                            DraggedItem.Equipped = false;
                        }
                        if (results[0].gameObject.GetComponent<Item>() != null)
                        {
                            if((results[0].gameObject.GetComponent<Item>().Equipped && results[0].gameObject.GetComponent<Item>().Type.Type == DraggedItem.Type.Type) && results[0].gameObject.GetComponent<Item>() != DraggedItem)
                            Debug.Log("Replaced Item");
                            DraggedItem.gameObject.transform.SetParent(results[0].gameObject.transform.parent);
                            DraggedItem.transform.position = results[0].gameObject.transform.parent.position;
                            DraggedItem.Equipped = true;
                            results[0].gameObject.GetComponent<Item>().Equipped = false;
                            results[0].gameObject.GetComponent<Item>().transform.SetParent(StockList);
                            Hero.SelectedHero.UnequipWeapon(results[0].gameObject.GetComponent<Weapon>());
                            Hero.SelectedHero.EquipWeapon(DraggedItem.GetComponent<Weapon>());
                            Hero.OpenEquipMenu();

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
                CurrentItem.transform.parent = ShopList;
                CurrentItem.Selling = true;
            }
            else
            {
                Debug.Log("No available slots in shop");
            }

        }
        else
        {
            Debug.Log("No Item selected");
        }
        CurrentItem = null;
        ItemInfoText.text = "SELECT AN ITEM";
    }

    public void SelectItem(Item i)
    {
        CurrentItem = i;
        if(i.GetComponent<Weapon>() != null)
        {
            ItemInfoText.text = i.GetComponent<Weapon>().WeaponName + "\n\n DMG: " + i.GetComponent<Weapon>().Damage + "\n\n" + i.Price +"G"; 
        }
    }

    public void CollectGold(float i)
    {
        Gold += i;
        GoldText.text = Gold + "G";
    }

    public void UpdatePrices()
    {
        
        
    }
}
