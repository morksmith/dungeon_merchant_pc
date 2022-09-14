using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    public ScrollingWindow TopContent;
    public ScrollingWindow BottomContent;
    void Start()
    {
        dragDistance = Screen.height * 10 / 100; //dragDistance is 15% height of the screen
    }

    void Update()
    {
        var RoomDistance = Screen.width * 30 /100;
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            //Debug.Log("Right Swipe" + fp.y);
                            if(fp.y > Screen.height / 2)
                            {
                                if(TopContent.CurrentWindow < 2)
                                {
                                    TopContent.ScrollRight();
                                }
                            }
                            else
                            {
                                if (BottomContent.CurrentWindow < 2)
                                {
                                    BottomContent.ScrollRight();
                                }
                            }
                        }
                        else
                        {   //Left swipe
                            //Debug.Log("Left Swipe");
                            if (fp.y > Screen.height / 2)
                            {
                                if (TopContent.CurrentWindow > 0)
                                {
                                    TopContent.ScrollLeft();
                                }
                            }
                            else
                            {
                                if (BottomContent.CurrentWindow > 0)
                                {
                                    BottomContent.ScrollLeft();
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                
            }
        }
    }
}
