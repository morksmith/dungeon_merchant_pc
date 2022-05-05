using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public Transform LockedTransform;
    // Start is called before the first frame update
    void Start()
    {
        LockedTransform = GameObject.Find("Dungeon Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = LockedTransform.forward;
    }
}
