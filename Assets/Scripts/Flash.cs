using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public float FlashTime = 0.5f;
    private SpriteRenderer rend;
    private float flashTimer;

    private void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if(flashTimer < FlashTime)
        {
            flashTimer += Time.deltaTime;
            rend.material.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.black, flashTimer/FlashTime));
        }
    }

    public void FlashWhite()
    {
        flashTimer = 0;
        rend.material.SetColor("_EmissionColor", Color.white);
    }
}
