using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bones : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 20 - Mathf.FloorToInt(transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
