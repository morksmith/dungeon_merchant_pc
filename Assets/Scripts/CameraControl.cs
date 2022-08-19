using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform Target;
    public float MoveSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + new Vector3(0,5,0), MoveSpeed * Time.deltaTime);
    }

    public void AddShake(float f)
    {
        transform.position += new Vector3(Random.Range(-f, f), 0, Random.Range(-f, f));
    }
}
