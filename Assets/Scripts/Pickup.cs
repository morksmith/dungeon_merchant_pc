using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    private SpriteRenderer rend;
    public float ScatterTime = 2;
    public float CollectTime = 1;
    private float scatterTimer;
    private float collectTimer;
    public bool IsChest = false;
    public bool Collecting = false;
    public AnimationCurve ScatterCurve;
    private Vector3 startPosition;
    public Vector3 CollectPosition;
    private Vector3 scatterPosition;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        var x = Random.Range(-1.0f, 1.0f);
        var z = Random.Range(-1.0f, 1.0f);
        Debug.Log(x + z);
        scatterPosition = transform.position + new Vector3(x, 0, z);
        startPosition = transform.position;
    }

    void Update()
    {
        if (!Collecting)
        {
            if (!IsChest)
            {
                rend.sortingOrder = 18 - Mathf.FloorToInt(transform.position.z);
            }
            else
            {
                rend.sortingOrder = 22 - Mathf.FloorToInt(transform.position.z);
            }
            if (scatterTimer < ScatterTime)
            {
                transform.position = Vector3.Lerp(startPosition, scatterPosition, ScatterCurve.Evaluate(scatterTimer / ScatterTime));
                scatterTimer += Time.deltaTime;
            }
            else
            {
                Collecting = true;
            }
        }
        else
        {
            if (collectTimer < CollectTime)
            {
                CollectPosition = Camera.main.transform.position;
                transform.position = Vector3.Lerp(transform.position, CollectPosition, collectTimer / CollectTime);
                collectTimer += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
       
        
    }
}
