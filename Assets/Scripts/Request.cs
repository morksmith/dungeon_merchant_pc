using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Request : MonoBehaviour
{
    public float MaxNumber;
    public float CurrentNumber;
    public enum RequestType
    {
        Beasts,
        Ghosts,
        Skellies,
        Demons,
        Swords,
        Clubs,
        Bows,
        Wands
    }
}
