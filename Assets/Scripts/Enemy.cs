using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float MaxHP;
    public float HP;
    public float Damage;
    public float XP;
    public float Range = 1;
    public float AgroRange = 5;
    public enum CurrentState
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    public CurrentState State;
    public float UpdateTime;

    private float step;
    private Transform hero;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hero = GameObject.FindObjectOfType<HeroAI>().transform;
        step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector3.Distance(transform.position, hero.position);
        if(State == CurrentState.Idle)
        {
            if(step < UpdateTime)
            {
                step += Time.deltaTime;
                
            }
            else
            {
                if(dist < AgroRange)
                {
                    State = CurrentState.Moving;
                    agent.SetDestination(hero.position);
                    agent.isStopped = false;
                    step = 0;
                }
                if(dist < Range)
                {
                    State = CurrentState.Attacking;
                    agent.isStopped = true;
                    step = 0;
                }
                step = 0;
            }
        }
        if(State == CurrentState.Moving)
        {
            if (step < UpdateTime)
            {
                step += Time.deltaTime;

            }
            else
            {
                if (dist < Range)
                {
                    State = CurrentState.Attacking;
                    agent.isStopped = true;
                    step = 0;
                }
                else
                {
                    agent.SetDestination(hero.position);
                    agent.isStopped = false;
                    step = 0;
                }
            }
        }
        if (State == CurrentState.Attacking)
        {
            if (step < UpdateTime)
            {
                step += Time.deltaTime;
            }
            else
            {
                if (dist < Range)
                {
                    Attack();
                    agent.isStopped = true;
                    step = 0;
                }
                step = 0;
            }
        }
    }

    public void TakeDamage(float i)
    {
        HP -= i;
    }

    public void Attack()
    {
        if (hero.GetComponent<Stats>().HP > Damage)
        {
            hero.GetComponent<HeroAI>().TakeDamage(Damage);
            Debug.Log("Hero Takes " + Damage + " Damage!");
        }
        else
        {
            Destroy(hero.gameObject);
            State = CurrentState.Idle;
            step = 0;
        }
        step = 0;
    }
}
