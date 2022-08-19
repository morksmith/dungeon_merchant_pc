using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeAction : MonoBehaviour
{
    public UnityEvent EventToInvoke;

    public void InvokeEvents()
    {
        EventToInvoke.Invoke();
    }
}
