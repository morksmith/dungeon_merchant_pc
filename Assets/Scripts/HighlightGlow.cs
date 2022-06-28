using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightGlow : MonoBehaviour
{
    public Image Highlight;

    public Color LowColour;
    public Color HighColour;

    public float Frequency;

    private float step;

    // Update is called once per frame
    void Update()
    {
        step += Time.deltaTime * Frequency;

        Highlight.color = Color.Lerp(LowColour, HighColour, Mathf.Sin(step));
    }
}
