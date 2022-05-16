using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Level;
    public int Rarity = 0;
    public string WeaponName;
    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
        Damage += (Level * 2);
        gameObject.GetComponent<Item>().Price = Damage * 8;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
