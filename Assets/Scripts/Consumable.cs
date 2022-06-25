using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public enum ConsumableType
    {
        Potion,
        Portal,
    }
    public ConsumableType Type;
    public float Level;
    public float Value;

    private void Start()
    {
        
        
    }
}
