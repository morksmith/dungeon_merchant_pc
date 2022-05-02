using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingWindow : MonoBehaviour
{
    public RectTransform Rect;
    public int CurrentWindow = 1;
    public float ScrollSpeed;
    public List<Vector2> WindowPositions;

    public Vector2 startPos;
    public Vector2 targetPos;

    private float step;
    // Start is called before the first frame update
    void Start()
    {
        startPos = Rect.anchoredPosition;
        targetPos = Rect.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {        
        if(step < 1)
        {
            step += Time.deltaTime;
            var dist = Mathf.Clamp01(step / ScrollSpeed);
            Debug.Log(dist);
            Rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, dist);
        }
    }

    public void ScrollRight()
    {
        if(CurrentWindow < WindowPositions.Count -1)
        {
            CurrentWindow++;
            startPos = Rect.anchoredPosition;
            targetPos = WindowPositions[CurrentWindow];
            step = 0;
        }
    }
    public void ScrollLeft()
    {
        if (CurrentWindow > 0)
        {
            CurrentWindow--;
            startPos = Rect.anchoredPosition;
            targetPos = WindowPositions[CurrentWindow];
            step = 0;
        }
    }
    public void SetWindow(int i)
    {
        CurrentWindow = i;
        startPos = Rect.anchoredPosition;
        targetPos = WindowPositions[CurrentWindow];
        step = 0;
    }
}
