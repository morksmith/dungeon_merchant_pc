using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class HeroAI : MonoBehaviour
{

    public enum HeroState
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    public HeroState State;
    public Stats Stats;
    public Transform CurrentTarget;
    public NavMeshAgent Agent;
    public float UpdateTime = 1;
    public Vector3 TargetPos;
    public bool LevelCleared = false;
    public GameObject FloatingNumber;


    private float step;
    private Enemy[] enemies;
    private Transform exit;
    // Start is called before the first frame update
    void Start()
    {
        exit = GameObject.FindObjectOfType<Exit>().transform;
        LookForEnemies();
    }
    // Update is called once per frame
    void Update()
    {
        if(State == HeroState.Idle)
        {
            if(step < UpdateTime)
            {
                step += Time.deltaTime;
            }
            else
            {
                step = 0;
                LookForEnemies();

            }            
        }
        if(State == HeroState.Moving)
        {
            if(CurrentTarget != null)
            {
                var targetDist = Vector3.Distance(transform.position, CurrentTarget.position);
                if (targetDist > Stats.Range)
                {
                    Agent.SetDestination(CurrentTarget.position);
                }
                else
                {
                    State = HeroState.Attacking;
                    Agent.isStopped = true;
                    step = 0;
                }

            }
            else
            {
                if (LevelCleared)
                {
                    if(Vector2.Distance(transform.position, TargetPos) < 0.1f)
                    {
                        Debug.Log("Hero Exited the Level");
                    }
                }
            }
        }
        if(State == HeroState.Attacking)
        {
            if (step < UpdateTime)
            {
                step += Time.deltaTime;
            }
            else
            {
                Attack();
            }
        }
    }

    public void LookForEnemies()
    {
        float dist = 999;
        enemies = GameObject.FindObjectsOfType<Enemy>();
        if(enemies.Length > 0)
        {
            for (var i = 0; i < enemies.Length; i++)
            {
                if(Vector2.Distance(transform.position, enemies[i].transform.position) < dist)
                {
                    CurrentTarget = enemies[i].transform;
                    TargetPos = CurrentTarget.position;
                    State = HeroState.Moving;
                    Agent.isStopped = false;
                    dist = Vector2.Distance(transform.position, enemies[i].transform.position);

                }
            }
        }
        else
        {
            LevelCleared = true;
            CurrentTarget = null;
            TargetPos = exit.position;
            Agent.SetDestination(exit.position);
            State = HeroState.Moving;
            Agent.isStopped = false;
        }
        

    }

    public void Attack()
    {
        if(CurrentTarget.GetComponent<Enemy>().HP > Stats.Damage)
        {
            CurrentTarget.GetComponent<Enemy>().TakeDamage(Stats.Damage);
            var newNumber = Instantiate(FloatingNumber, CurrentTarget.position, Quaternion.Euler(Vector3.forward));
            newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "-" + Stats.Damage;
        }
        else
        {
            Destroy(CurrentTarget.gameObject);
            CurrentTarget = null;
            State = HeroState.Idle;
            step = 0;
        }
        step = 0;
    }

    public void TakeDamage(float i)
    {
        Stats.HP -= i;
        var newNumber = Instantiate(FloatingNumber, transform.position, Quaternion.Euler(Vector3.forward));
        newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "-" + i;
    }
}
