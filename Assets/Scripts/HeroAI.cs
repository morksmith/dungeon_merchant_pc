using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

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
    public bool Active = false;
    public bool Waiting = true;
    public Stats Stats;
    public Transform CurrentTarget;
    public NavMeshAgent Agent;
    public float UpdateTime = 1;
    public Vector3 TargetPos;
    public bool LevelCleared = false;
    public SpriteRenderer HeroSprite;
    public GameObject FloatingNumber;
    public HeroManager Manager;
    public DungeonManager DM;
    private float activeTimer;
    private float step;
    private Enemy[] enemies;
    private Transform exit;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = DM.HeroStartPosition;

        exit = GameObject.FindObjectOfType<Exit>().transform;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Waiting == true)
        {
            return;
        }
        if (!Active)
        {
            activeTimer += Time.deltaTime;
            if (activeTimer > UpdateTime)
            {
                LookForEnemies();
                Active = true;
                activeTimer = 0;
            }
        }

        HeroSprite.sortingOrder = 20 - Mathf.FloorToInt(transform.position.z);
        if (Agent.destination.x < transform.position.x)
        {
            HeroSprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            HeroSprite.transform.localScale = new Vector3(1, 1, 1);
        }

        if (State == HeroState.Idle)
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

                        Manager.MaxDungeonFloor++;
                        Manager.CheckFloorButtons();
                        Active = false;
                        Waiting = true;
                        LevelCleared = false;
                        CurrentTarget = null;
                        Agent.isStopped = true;
                        Waiting = true;
                        DM.Running = false;
                        State = HeroState.Idle;
                        DM.DungeonCompleted = true;
                        DM.DungeonComplete();
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
        if(CurrentTarget == null)
        {
            return;
        }
        if (Stats.DamageType == CurrentTarget.GetComponent<Enemy>().DamageWeakness)
        {
            var dmg = Mathf.CeilToInt(Stats.Damage * 1.5f);
            CurrentTarget.GetComponent<Enemy>().TakeDamage(dmg);
            var newNumber = Instantiate(FloatingNumber, CurrentTarget.position, Quaternion.Euler(Vector3.forward));
            newNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "-" + dmg;
        }
        else
        {
            var dmg = Stats.Damage;
            CurrentTarget.GetComponent<Enemy>().TakeDamage(dmg);
            var newNumber = Instantiate(FloatingNumber, CurrentTarget.position, Quaternion.Euler(Vector3.forward));
            newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "-" + dmg;
        }
        if (CurrentTarget.GetComponent<Enemy>().HP <= 0)
        {
            Stats.XP += CurrentTarget.GetComponent<Enemy>().XP;
            var GoldFound = Mathf.CeilToInt(CurrentTarget.GetComponent<Enemy>().Gold * Stats.Discovery * DM.GoldBonus);
            Stats.GoldHeld += GoldFound;
            Destroy(CurrentTarget.gameObject);
            CurrentTarget = null;
            State = HeroState.Idle;
            step = 0;


        }
        step = 0;
    }

    public void TakeDamage(float i, Enemy e)
    {
        if(Stats.HP > i)
        {
            Stats.HP -= i;
            var newNumber = Instantiate(FloatingNumber, transform.position, Quaternion.Euler(Vector3.forward));
            newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "-" + i;
            newNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            e.Hero = null;
            Die();
        }
       

    }

    public void Die()
    {
        Active = false;
        Waiting = true;
        LevelCleared = false;
        CurrentTarget = null;
        Agent.isStopped = true;
        Waiting = true;
        DM.Running = false;
        State = HeroState.Dead;
        Stats.Die();
        DM.DungeonCompleted = false;
        DM.DungeonComplete();

    }

    


}
