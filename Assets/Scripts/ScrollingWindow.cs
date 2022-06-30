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

    public bool NewMerchant = false;
    public bool NewHero = false;
    public bool NewSale = false;
    public bool NewItem = false;

    public GameObject LeftIcon;
    public GameObject RightIcon;

    private float step;
    // Start is called before the first frame update
    void Start()
    {
        startPos = Rect.anchoredPosition;
        targetPos = Rect.anchoredPosition;
        CheckIcons();
    }

    // Update is called once per frame
    void Update()
    {
        if (step < 1)
        {
            step += Time.deltaTime;
            var dist = Mathf.Clamp01(step / ScrollSpeed);
            Rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, dist);
        }
    }

    public void ScrollRight()
    {
        if (CurrentWindow < WindowPositions.Count - 1)
        {
            CurrentWindow++;
            startPos = Rect.anchoredPosition;
            targetPos = WindowPositions[CurrentWindow];
            step = 0;
        }
        CheckIcons();
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
        CheckIcons();

    }
    public void SetWindow(int i)
    {
        CurrentWindow = i;
        startPos = Rect.anchoredPosition;
        targetPos = WindowPositions[CurrentWindow];
        step = 0;
        CheckIcons();

    }
    public void NewIcon()
    {
        if (NewSale)
        {
            if(CurrentWindow < 1)
            {
                LeftIcon.SetActive(true);
            }
            else if(CurrentWindow > 1)
            {
                RightIcon.SetActive(true);
            }
        }
        if (NewMerchant)
        {
            if (CurrentWindow > 0)
            {
                RightIcon.SetActive(true);
            }
            
        }
        if (NewHero)
        {
            if (CurrentWindow > 0)
            {
                RightIcon.SetActive(true);
            }
        }
        if (NewItem)
        {
            if(CurrentWindow < 2)
            {
                LeftIcon.SetActive(true);
            }
        }
    }
    public void CheckIcons()
    {
        if(NewSale && CurrentWindow == 1)
        {
            NewSale = false;
        }
        if (NewMerchant && CurrentWindow == 0)
        {
            NewMerchant = false;
        }
        if (NewHero && CurrentWindow == 0)
        {
            NewHero = false;
        }
        if (NewItem && CurrentWindow == 2)
        {
            NewItem = false;
        }
        RightIcon.SetActive(false);
        LeftIcon.SetActive(false);
        NewIcon();


    }
    public void NewMerchantIcon()
    {
        NewMerchant = true;
        CheckIcons();
    }
    public void NewHeroIcon()
    {
        NewHero = true;
        CheckIcons();
    }
    public void NewSaleIcon()
    {
        NewSale = true;
        CheckIcons();
    }
    public void NewItemIcon()
    {
        NewItem = true;
        CheckIcons();
    }


}
