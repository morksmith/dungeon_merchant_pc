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
        CreateHelm(l, merch);
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


}
