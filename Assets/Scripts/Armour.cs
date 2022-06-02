using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    public float Level;
    public float HP;

    private void Start()
    {
        HP += (Level * 2);
        gameObject.GetComponent<Item>().Price = HP * 6;
    }
}
