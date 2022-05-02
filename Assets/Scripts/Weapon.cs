using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Level;
    public string WeaponName;
    public enum WeaponType
    {
        Sword,
        Club,
        Bow,
        Wand
    }
    public WeaponType Type;
    public float Damage;
    public float Price;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
