using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    public Joystick Joy;
    public HeroAI Hero;

    public void Update()
    {
        if(Joy.Horizontal != 0 || Joy.Vertical != 0)
        {
            Vector3 moveVector = new Vector3(Joy.Horizontal, 0, Joy.Vertical);
            Hero.JoystickMove(moveVector);
        }
    }
}
