using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public StockManager Stock;
    public Transform MerchList;
    public Transform StockList;
    public GameObject PotionPrefab;
    public GameObject PortalPrefab;
    public List<Sprite> ItemSprites;
    public List<GameObject> SwordPrefabs;
    public List<GameObject> ClubPrefabs;
    public List<GameObject> BowPrefabs;
    public List<GameObject> WandPrefabs;
    public List<GameObject> ArmourPrefabs;
    public List<GameObject> HelmPrefabs;
    public List<GameObject> BootsPrefabs;
    public List<GameObject> ConsumablePrefabs;

    public void GenerateWeapon(int l, bool merch)
    {
        var pick = Random.Range(0, 4);
        if(pick == 0)
        {
            CreateSword(l, merch);
        }
        else if(pick == 1)
        {
            CreateClub(l, merch);
        }
        else if (pick == 2)
        {
            CreateBow(l, merch);
        }
        else if (pick == 3)
        {
            CreateWand(l, merch);
        }
        Stock.UpdatePrices();

    }

    public void GenerateArmour(int l, bool merch)
    {
        var pick = Random.Range(0, 2);
        if (pick == 0)
        {
            CreateHelm(l, merch);
        }
        else
        {
            CreateArmour(l, merch);
        }

    }

    public void GenerateConsumable(int l, bool merch)
    {
        var pick = Random.Range(0, 2);
        if (pick == 0)
        {
            CreatePotion(l, merch);
        }
        else
        {
            CreatePortal(l, merch);
        }
    }

    public void CreateSword(int l, bool merch)
    {
        Debug.Log("Created New Sword");
        var pick = Random.Range(0, SwordPrefabs.Count);
        var newSword = Instantiate(SwordPrefabs[pick], StockList);
        newSword.GetComponent<Weapon>().Level = l;
        if (merch)
        {
            newSword.transform.SetParent(MerchList);
            newSword.GetComponent<Item>().Merchant = true;

        }
        newSword.GetComponent<Item>().SetStats();


    }

    public void CreateClub(int l, bool merch)
    {
        Debug.Log("Created New Club");
        var pick = Random.Range(0, ClubPrefabs.Count);
        var newClub = Instantiate(ClubPrefabs[pick], StockList);
        newClub.GetComponent<Weapon>().Level = l;
        if(merch)
        {
            newClub.transform.SetParent(MerchList);
            newClub.GetComponent<Item>().Merchant = true;


        }
        newClub.GetComponent<Item>().SetStats();


    }
    public void CreateBow(int l, bool merch)
    {
        Debug.Log("Created New Bow");
        var pick = Random.Range(0, BowPrefabs.Count);
        var newBow = Instantiate(BowPrefabs[pick], StockList);
        newBow.GetComponent<Weapon>().Level = l;
        if (merch)
        {
            newBow.transform.SetParent(MerchList);
            newBow.GetComponent<Item>().Merchant = true;

        }
        newBow.GetComponent<Item>().SetStats();


    }
    public void CreateWand(int l, bool merch)
    {
        Debug.Log("Created New Wand");
        var pick = Random.Range(0, WandPrefabs.Count);
        var newWand = Instantiate(WandPrefabs[pick], StockList);
        newWand.GetComponent<Weapon>().Level = l;
        if (merch)
        {
            newWand.transform.SetParent(MerchList);
            newWand.GetComponent<Item>().Merchant = true;

        }
        newWand.GetComponent<Item>().SetStats();


    }
    public void CreateHelm(int l, bool merch)
    {
        Debug.Log("Created New Helm");
        var pick = Random.Range(0, HelmPrefabs.Count);
        var newHelm = Instantiate(HelmPrefabs[pick], StockList);
        newHelm.GetComponent<Armour>().Level = l;
        if (merch)
        {
            newHelm.transform.SetParent(MerchList);
            newHelm.GetComponent<Item>().Merchant = true;

        }
        newHelm.GetComponent<Item>().SetStats();

    }
    public void CreateArmour(int l, bool merch)
    {
        Debug.Log("Created New Armour");
        var pick = Random.Range(0, ArmourPrefabs.Count);
        var newArmour = Instantiate(ArmourPrefabs[pick], StockList);
        newArmour.GetComponent<Armour>().Level = l;
        if (merch)
        {
            newArmour.transform.SetParent(MerchList);
            newArmour.GetComponent<Item>().Merchant = true;

        }
        newArmour.GetComponent<Item>().SetStats();

    }
    public void CreatePotion(int l, bool merch)
    {
        Debug.Log("Created New Potion");
        var newPotion = Instantiate(PotionPrefab, StockList);
        newPotion.GetComponent<Consumable>().Level = l;
        if (merch)
        {
            newPotion.transform.SetParent(MerchList);
            newPotion.GetComponent<Item>().Merchant = true;
        }
        newPotion.GetComponent<Item>().SetStats();

    }
    public void CreatePortal(int l, bool merch)
    {
        Debug.Log("Created New Portal");
        var newPortal = Instantiate(PortalPrefab, StockList);
        newPortal.GetComponent<Consumable>().Level = l;
        if (merch)
        {
            newPortal.transform.SetParent(MerchList);
            newPortal.GetComponent<Item>().Merchant = true;
        }
        newPortal.GetComponent<Item>().SetStats();

    }

    public void CreateSpecificItem(ItemData id)
    {
        if (id.TypeIndex < 4)
        {
            var newWeaponItem = new GameObject();
            if (id.TypeIndex == 0)
            {
                var newSword = Instantiate(SwordPrefabs[0]);
                newWeaponItem = newSword;
            }
            else if (id.TypeIndex == 1)
            {
                var newClub = Instantiate(ClubPrefabs[0]);
                newWeaponItem = newClub;
            }
            else if (id.TypeIndex == 2)
            {
                var newBow = Instantiate(BowPrefabs[0]);
                newWeaponItem = newBow;
            }
            else if (id.TypeIndex == 3)
            {
                var newWand = Instantiate(WandPrefabs[0]);
                newWeaponItem = newWand;
            }

            newWeaponItem.name = id.ItemName;
            var newItem = newWeaponItem.GetComponent<Item>();
            var newWeapon = newWeaponItem.GetComponent<Weapon>();
            newItem.ItemName = id.ItemName;
            newItem.SpriteIndex = id.SpriteIndex;
            newItem.TypeIndex = id.TypeIndex;
            newItem.BasePrice = id.BasePrice;
            newWeapon.Level = id.Level;
            newWeapon.WeaponName = id.ItemName;
            newWeapon.Damage = id.StatPoint;
            newItem.Selling = id.Selling;
            newItem.SellTimer = id.SellTimer;
            newItem.ItemSprite.sprite = ItemSprites[newItem.SpriteIndex];
            if (id.Merchant)
            {
                newItem.transform.SetParent(MerchList);
                newItem.GetComponent<Item>().Merchant = true;
                newItem.Price = id.BasePrice;
                    newItem.SellTime = 0;
                    newItem.transform.localScale = Vector3.one;
            }
            else
            {
                if (!id.Selling)
                {
                    newItem.transform.SetParent(StockList);
                    newItem.Price = id.BasePrice;
                    newItem.SellTime = 0;
                    newItem.transform.localScale = Vector3.one;
                }
                else
                {
                    newItem.Selling = true;
                    newItem.transform.SetParent(Stock.ShopList);
                    newItem.Price = newItem.BasePrice;
                    newItem.SellTime = newItem.BasePrice;
                    newItem.SellTimer = id.SellTimer;
                    newItem.transform.localScale = Vector3.one;

                }
                if (id.Sold)
                {
                    newItem.transform.SetParent(Stock.ShopList);
                    newItem.Price = newItem.BasePrice;
                    newItem.SellTime = newItem.BasePrice;
                    newItem.SellTimer = newItem.SellTime;
                    newItem.ReadyToSell();
                }
            }
            
            

            newItem.PriceUI();
                

        }
        else if(id.TypeIndex < 6)
        {
            var newArmourItem = new GameObject();
            if (id.TypeIndex == 4)
            {
                var helm = Instantiate(HelmPrefabs[0]);
                newArmourItem = helm;
            }
            else if (id.TypeIndex == 5)
            {
                var armour = Instantiate(ArmourPrefabs[0]);
                newArmourItem = armour;
            }
            var newItem = newArmourItem.GetComponent<Item>();
            var newArmour = newArmourItem.GetComponent<Armour>();
            newItem.ItemName = id.ItemName;
            newItem.SpriteIndex = id.SpriteIndex;
            newItem.BasePrice = id.BasePrice;
            newItem.TypeIndex = id.TypeIndex;
            newArmour.Level = id.Level;
            newArmour.HP = id.StatPoint;
            newItem.Selling = id.Selling;
            newItem.SellTimer = id.SellTimer;
            newItem.ItemSprite.sprite = ItemSprites[newItem.SpriteIndex];
            if (id.Merchant)
            {
                newItem.transform.SetParent(MerchList);
                newItem.GetComponent<Item>().Merchant = true;
                newItem.Price = id.BasePrice;
                newItem.SellTime = 0;
                newItem.transform.localScale = Vector3.one;
            }
            else
            {
                if (!id.Selling)
                {
                    newItem.transform.SetParent(StockList);
                    newItem.Price = id.BasePrice;
                    newItem.SellTime = 0;
                    newItem.transform.localScale = Vector3.one;
                }
                else
                {
                    newItem.Selling = true;
                    newItem.transform.SetParent(Stock.ShopList);
                    newItem.Price = newItem.BasePrice;
                    newItem.SellTime = newItem.BasePrice;
                    newItem.SellTimer = id.SellTimer;
                    newItem.transform.localScale = Vector3.one;

                }
                if (id.Sold)
                {
                    newItem.transform.SetParent(Stock.ShopList);
                    newItem.Price = newItem.BasePrice;
                    newItem.SellTime = newItem.BasePrice;
                    newItem.SellTimer = newItem.SellTime;
                    newItem.ReadyToSell();
                }
                newItem.PriceUI();
            }

            newItem.PriceUI();

        }
        else
        {
            var newConsumableItem = new GameObject();
            if (id.ConsumableType == 1)
            {
                var potion = Instantiate(PotionPrefab);
                newConsumableItem = potion;
            }
            else if (id.ConsumableType == 2)
            {
                var portal = Instantiate(PortalPrefab);
                newConsumableItem = portal;
            }
            var newItem = newConsumableItem.GetComponent<Item>();
            var newConsumable = newConsumableItem.GetComponent<Consumable>();
            newItem.ItemName = id.ItemName;
            newItem.SpriteIndex = id.SpriteIndex;
            newItem.BasePrice = id.BasePrice;
            newItem.TypeIndex = id.TypeIndex;
            newConsumable.Level = id.Level;
            newConsumable.Value = id.StatPoint;
            newItem.Selling = id.Selling;
            newItem.SellTimer = id.SellTimer;
            newItem.ItemSprite.sprite = ItemSprites[newItem.SpriteIndex];
            if (id.Merchant)
            {
                newItem.transform.SetParent(MerchList);
                newItem.GetComponent<Item>().Merchant = true;
                newItem.Price = id.BasePrice;
                newItem.SellTime = 0;
                newItem.transform.localScale = Vector3.one;
            }
            else
            {
                if (!id.Selling)
                {
                    newItem.transform.SetParent(StockList);
                    newItem.Price = id.BasePrice;
                    newItem.SellTime = 0;
                    newItem.transform.localScale = Vector3.one;
                }
                else
                {
                    newItem.Selling = true;
                    newItem.transform.SetParent(Stock.ShopList);
                    newItem.Price = newItem.BasePrice;
                    newItem.SellTime = newItem.BasePrice;
                    newItem.SellTimer = id.SellTimer;
                    newItem.transform.localScale = Vector3.one;

                }
                if (id.Sold)
                {
                    newItem.transform.SetParent(Stock.ShopList);
                    newItem.Price = newItem.BasePrice;
                    newItem.SellTime = newItem.BasePrice;
                    newItem.SellTimer = newItem.SellTime;
                    newItem.ReadyToSell();
                }
                newItem.PriceUI();
            }

            newItem.PriceUI();
        }
           
    }
    public void CreateEquippedItem(ItemData id, Transform parent, Stats hero)
    {
        if (id.TypeIndex < 4)
        {
            var newWeaponItem = new GameObject();
            if (id.TypeIndex == 0)
            {
                var newSword = Instantiate(SwordPrefabs[0]);
                newWeaponItem = newSword;
            }
            else if (id.TypeIndex == 1)
            {
                var newClub = Instantiate(ClubPrefabs[0]);
                newWeaponItem = newClub;
            }
            else if (id.TypeIndex == 2)
            {
                var newBow = Instantiate(BowPrefabs[0]);
                newWeaponItem = newBow;
            }
            else if (id.TypeIndex == 3)
            {
                var newWand = Instantiate(WandPrefabs[0]);
                newWeaponItem = newWand;
            }

            newWeaponItem.name = id.ItemName;
            var newItem = newWeaponItem.GetComponent<Item>();
            var newWeapon = newWeaponItem.GetComponent<Weapon>();
            newItem.ItemName = id.ItemName;
            newItem.SpriteIndex = id.SpriteIndex;
            newItem.BasePrice = id.BasePrice;
            newItem.TypeIndex = id.TypeIndex;
            newWeapon.Level = id.Level;
            newWeapon.WeaponName = id.ItemName;
            newWeapon.Damage = id.StatPoint;
            newItem.Selling = id.Selling;
            newItem.SellTimer = id.SellTimer;
            newItem.ItemSprite.sprite = ItemSprites[newItem.SpriteIndex];
            newItem.transform.SetParent(parent);
            newItem.transform.position = parent.position;
            newItem.transform.localScale = Vector3.one;
            newItem.Equipped = true;
            newItem.PriceInfo.SetActive(false);
            hero.WeaponItem = newItem;
            newItem.PriceUI();
        }
        else if (id.TypeIndex < 6)
        {
            var newArmourItem = new GameObject();
            if (id.TypeIndex == 4)
            {
                var helm = Instantiate(HelmPrefabs[0]);
                newArmourItem = helm;
                hero.HelmItem = newArmourItem.GetComponent<Item>();
            }
            else if (id.TypeIndex == 5)
            {
                var armour = Instantiate(ArmourPrefabs[0]);
                newArmourItem = armour;
                hero.ArmourItem = newArmourItem.GetComponent<Item>();
            }
            var newItem = newArmourItem.GetComponent<Item>();
            var newArmour = newArmourItem.GetComponent<Armour>();
            newItem.ItemName = id.ItemName;
            newItem.SpriteIndex = id.SpriteIndex;
            newItem.BasePrice = id.BasePrice;
            newItem.TypeIndex = id.TypeIndex;
            newArmour.Level = id.Level;
            newArmour.HP = id.StatPoint;
            newItem.Selling = id.Selling;
            newItem.SellTimer = id.SellTimer;
            newItem.ItemSprite.sprite = ItemSprites[newItem.SpriteIndex];
            newItem.transform.SetParent(parent);
            newItem.transform.position = parent.position;
            newItem.transform.localScale = Vector3.one;
            newItem.Equipped = true;
            newItem.PriceInfo.SetActive(false);
            newItem.PriceUI();


        }
        else
        {
            var newConsumableItem = new GameObject();
            if (id.ConsumableType == 1)
            {
                var potion = Instantiate(PotionPrefab);
                newConsumableItem = potion;
            }
            else if (id.ConsumableType == 2)
            {
                var portal = Instantiate(PortalPrefab);
                newConsumableItem = portal;
            }
            var newItem = newConsumableItem.GetComponent<Item>();
            var newConsumable = newConsumableItem.GetComponent<Consumable>();
            newItem.ItemName = id.ItemName;
            newItem.SpriteIndex = id.SpriteIndex;
            newItem.BasePrice = id.BasePrice;
            newItem.TypeIndex = id.TypeIndex;
            newConsumable.Level = id.Level;
            newConsumable.Value = id.StatPoint;
            newItem.Selling = id.Selling;
            newItem.SellTimer = id.SellTimer;
            newItem.ItemSprite.sprite = ItemSprites[newItem.SpriteIndex];
            newItem.transform.SetParent(parent);
            newItem.transform.position = parent.position;
            newItem.transform.localScale = Vector3.one;
            newItem.Equipped = true;
            newItem.PriceInfo.SetActive(false);

            hero.ConsumableItem = newItem;
            newItem.PriceUI();
        }
    }




}
