using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LateAppear : MonoBehaviour
{
    private Image img;
    public float AppearTime = 1.5f;
    private float appearTimer;

    private void Start()
    {
        img = GetComponent<Image>();
    }
    public void Update()
    {
        if (gameObject.activeSelf)
        {
            if(appearTimer < AppearTime)
            {
                img.enabled = false;
                appearTimer += Time.deltaTime;
            }
            else
            {
                img.enabled = true;
            }
        }
    }
}
