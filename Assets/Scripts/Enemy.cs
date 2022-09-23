using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public bool Survival = false;
    public int Level = 1;
    public float MaxHP;
    public float HP;
    public float Damage;
    public int DamageWeakness;
    public float XP;
    public float Range = 1;
    public float AgroRange = 5;
    public float Gold;
    public bool Agro = false;
    public enum CurrentState
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    public CurrentState State;
    public float UpdateTime;
    private float dist;
    private float step;
    public Transform Hero;
    public SpriteRenderer EnemySprite;
    public Slider HPSlider;
    public AudioClip DeathSound;
    private SFXManager sfx;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(GameObject.FindObjectOfType<HeroAI>()!= null)
        {
            Hero = GameObject.FindObjectOfType<HeroAI>().transform;
        }
        step = 0;
        MaxHP = MaxHP + (Level * (Level *2));
        Damage = Damage + (Level * 2);
        XP = MaxHP + (Level * 2);
        HP = MaxHP;
        Gold = Random.Range(1, Damage);
        HPSlider.value = 1;
        sfx = GameObject.FindObjectOfType<SFXManager>();

    }

    // Update is called once per frame
    void Update()
    {
        EnemySprite.sortingOrder = 20 - Mathf.FloorToInt(transform.position.z);
        if(agent.destination.x > transform.position.x)
        {
            EnemySprite.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Hero == null)
        {
            State = CurrentState.Idle;
           
        }
        else
        {
            if (Survival)
            {
                if (!Hero.GetComponent<HeroAI>().DM.Running || Hero.GetComponent<HeroAI>().Waiting)
                {
                    return;
                }
                if (Agro)
                {
                    agent.SetDestination(Hero.position);
                    agent.isStopped = false;
                }
            }
            dist = Vector3.Distance(transform.position, Hero.position);
            if (State == CurrentState.Idle)
            {
                if (step < UpdateTime)
                {
                    step += Time.deltaTime;
                }
                else
                {
                    if (dist < AgroRange)
                    {
                        State = CurrentState.Moving;
                        agent.SetDestination(Hero.position);
                        agent.isStopped = false;
                        Agro = true;
                        step = 0;
                    }
                    if (dist < Range)
                    {
                        State = CurrentState.Attacking;
                        if (!Survival)
                        {
                            agent.isStopped = true;
                            step = 0;
                        }
                        else
                        {
                            agent.isStopped = true;
                            agent.SetDestination(Hero.position);
                        }
                        

                    }
                    step = 0;
                }
            }
            if (State == CurrentState.Moving)
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
                        if (!Survival)
                        {
                            agent.isStopped = true;
                        }
                        else
                        {
                            agent.isStopped = true;
                            agent.SetDestination(Hero.position);
                        }
                        step = 0;
                    }
                    else
                    {
                        agent.SetDestination(Hero.position);
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
                    }
                    else
                    {
                        State = CurrentState.Moving;
                        agent.SetDestination(Hero.position);
                        agent.isStopped = false;

                    }
                    step = 0;
                }
            }
        }
        
        
    }

    public void TakeDamage(float i)
    {
        
        HP -= i;
        HPSlider.value = HP / MaxHP;
        GetComponent<Flash>().FlashWhite();
        if (Survival)
        {
            agent.SetDestination(Hero.position);
        }
                
    }

    public void Attack()
    {
        if (!Hero.GetComponent<HeroAI>().DM.Running)
        {
            return;
        }
        Hero.GetComponent<HeroAI>().TakeDamage(Damage, this);
        Debug.Log("Hero Takes " + Damage + " Damage!");
       
        step = 0;
    }

    public void Die()
    {
        sfx.PlaySound(DeathSound);
        Destroy(this.gameObject);
    }
}
