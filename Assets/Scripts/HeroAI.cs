using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroAI : MonoBehaviour
{
    public NavMeshAgent Agent;

    public Vector3 TargetPos;
    // Start is called before the first frame update
    void Start()
    {
        Agent.SetDestination(TargetPos);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
