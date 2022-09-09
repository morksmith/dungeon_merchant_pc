using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public bool Occludes = false;
    public List<Sprite> PropSprites;
    public List<bool> PropOccludes;
    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        var pick = Random.Range(0, PropSprites.Count);
        rend.sprite = PropSprites[pick];
        Occludes = PropOccludes[pick];
        if (Occludes)
        {
            rend.sortingOrder = 20 - Mathf.FloorToInt(transform.position.z);
        }
        else
        {
            rend.sortingOrder = 2;
        }

    }
    void Update()
    {
        
    }

    public void SetSprite()
    {
        var pick = Random.Range(0, PropSprites.Count);
        rend.sprite = PropSprites[pick];
        Occludes = PropOccludes[pick];
    }
}
