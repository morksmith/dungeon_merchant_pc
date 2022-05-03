using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNumber : MonoBehaviour
{
    public float MoveSpeed;
    public float MoveDistance;
    private Vector3 startPos;
    private Vector3 endPos;
    private float step;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + new Vector3(0, 0, MoveDistance);
    }

    // Update is called once per frame
    void Update()
    {
        
        step += Time.deltaTime * Vector3.Distance(transform.position, endPos);
        var pos = Mathf.Clamp01(step / MoveSpeed);
        if(pos < 0.9f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, pos);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
