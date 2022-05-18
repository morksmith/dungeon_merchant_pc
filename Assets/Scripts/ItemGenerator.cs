using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public StockManager Stock;
    public Transform MerchList;
    public Transform StockList;
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

    }

    public void GenerateArmour(int l)
    {

    }

    public void GenerateConsumable()
    {

    }

    public void CreateSword(int l, bool merch)
    {
        Debug.Log("Created New Sword");
        var pick = Random.Range(0, SwordPrefabs.Count);
        var newSword = Instantiate(SwordPrefabs[pick], transform);
        newSword.GetComponent<Weapon>().Level = l;
        if (merch)
        {
            newSword.transform.SetParent(MerchList);
            newSword.GetComponent<Item>().Merchant = true;

        }
        else
        {
            newSword.transform.SetParent(StockList);
        }

    }

    public void CreateClub(int l, bool merch)
    {
        Debug.Log("Created New Club");
        var pick = Random.Range(0, ClubPrefabs.Count);
        var newClub = Instantiate(ClubPrefabs[pick], transform);
        newClub.GetComponent<Weapon>().Level = l;
        if(merch)
        {
            newClub.transform.SetParent(MerchList);
            newClub.GetComponent<Item>().Merchant = true;

        }
        else
        {
            newClub.transform.SetParent(StockList);
        }

    }
    public void CreateBow(int l, bool merch)
    {
        Debug.Log("Created New Bow");
        var pick = Random.Range(0, BowPrefabs.Count);
        var newBow = Instantiate(BowPrefabs[pick], transform);
        newBow.GetComponent<Weapon>().Level = l;
        if (merch)
        {
            newBow.transform.SetParent(MerchList);
            newBow.GetComponent<Item>().Merchant = true;

        }
        else
        {
            newBow.transform.SetParent(StockList);
        }
    }
    public void CreateWand(int l, bool merch)
    {
        Debug.Log("Created New Wand");
        var pick = Random.Range(0, WandPrefabs.Count);
        var newWand = Instantiate(WandPrefabs[pick], transform);
        newWand.GetComponent<Weapon>().Level = l;
        if (merch)
        {
            newWand.transform.SetParent(MerchList);
            newWand.GetComponent<Item>().Merchant = true;

        }
        else
        {
            newWand.transform.SetParent(StockList);
        }
    }

}
