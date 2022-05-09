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
        if(DraggedItem != null)
        {
            ItemBox.gameObject.SetActive(true);
            ItemBox.GetComponent<Image>().sprite = DraggedItem.ItemSprite.sprite;
        }
        else
        {
            ItemBox.gameObject.SetActive(false);

        }
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
                        if(results[0].gameObject.GetComponent<Item>() != null)
                        {
                            DraggedItem = results[0].gameObject.GetComponent<Item>();
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    ItemBox.position = touch.position;
                    break;

                case TouchPhase.Ended:
                    eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                    eventDataCurrentPosition.position = touch.position;
                    results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                    if (results.Count > 0)
                    {
                        Debug.Log("dropped on" + results[0].gameObject.name);
                        if (results[0].gameObject.GetComponent<EquipmentSlot>() != null)
                        {
                            Debug.Log("Dropped on Equipment Slot");
                            DraggedItem.gameObject.transform.parent = results[0].gameObject.transform;
                            DraggedItem.transform.position = results[0].gameObject.transform.position;
                        }
                    }
                    DraggedItem = null;
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
    }

    public void SelectItem(Item i)
    {
        CurrentItem = i;
        if(i.Type == Item.ItemType.Weapon)
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
        
        for(var i = 0; i < ItemList.Count; i++)
        {
            if(ItemList[i].Type == Item.ItemType.Weapon)
            {
                if(ItemList[i].GetComponent<Weapon>().Type == Weapon.WeaponType.Sword)
                {
                    ItemList[i].Price *= SwordPrice;
                }
            }
        }
    }
}
