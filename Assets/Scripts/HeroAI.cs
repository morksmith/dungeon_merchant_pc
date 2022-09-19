using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class HeroAI : MonoBehaviour
{
    public bool PlayerControlled = false;
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
    public AudioClip DeathSound;
    public AudioClip ChestSound;
    public AudioClip LevelUpSound;
    public AudioClip HealSound;
    public AudioClip TeleportSound;
    public GameObject EffectPrefab;
    public CameraControl Camera;
    public Animator DungeonOverlay;
    public GameObject BonesPrefab;
    public GameObject CoinPrefab;
    public GameObject ChestPrefab;
    public bool PortalOut = false;
    public float PortalTime = 1;
    private float portalTimer;
    

    private float activeTimer;
    private float step;
    private Enemy[] enemies;
    private Transform exit;
    private SFXManager sfx;

    private RequestManager requests;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = DM.HeroStartPosition;
        Camera.transform.position = transform.position + new Vector3(0, 5, 0);
        sfx = GameObject.FindObjectOfType<SFXManager>();
        exit = GameObject.FindObjectOfType<Exit>().transform;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerControlled)
        {

            //var moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //if(Vector3.Magnitude(moveVector)!= 0)
            //{
            //    Agent.SetDestination(transform.position + moveVector);
            //}
            
            if (CurrentTarget != null)
            {
                if(Vector3.Distance(transform.position, CurrentTarget.position) <= Stats.Range)
                {
                    State = HeroState.Attacking;
                }
            }
            
        }
        if (PortalOut)
        {
            if (portalTimer < PortalTime)
            {
                portalTimer += Time.deltaTime;
            }
            else
            {
                ExitPortal();
                portalTimer = 0;
                PortalOut = false;
            }
        }
        if (Waiting == true)
        {
            return;
        }
        if (!Active)
        {
            activeTimer += Time.deltaTime;
            if (activeTimer > UpdateTime)
            {
                exit = GameObject.FindObjectOfType<Exit>().transform;
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
            if (PlayerControlled)
            {
                if (step < UpdateTime)
                {
                    step += Time.deltaTime;
                }
                else
                {
                    step = 0;
                    LookForEnemies();

                }
                return;
            }
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
                }

            }
            else
            {
                if (LevelCleared)
                {
                    if(Vector2.Distance(transform.position, TargetPos) < 0.1f)
                    {


                        CompletedLevel();
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
                if(Vector3.Distance(transform.position, CurrentTarget.position) <= Stats.Range)
                {
                    Attack();
                }
                else
                {
                    CurrentTarget = null;
                    State = HeroState.Moving;
                    LookForEnemies();
                }
                
            }
        }
    }

    public void JoystickMove(Vector3 dir)
    {
        if (!Waiting && DM.Running)
        {
            Agent.SetDestination(transform.position + dir);
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
            if (!PlayerControlled)
            {
                TargetPos = exit.position;
                Agent.SetDestination(exit.position);
                State = HeroState.Moving;
                Agent.isStopped = false;
            }
            
        }
        

    }

    public void Attack()
    {
        Camera.AddShake(0.1f);
        var newEffect = Instantiate(EffectPrefab, CurrentTarget.position + new Vector3(0,0,0.5f), Quaternion.Euler(90, 0, 0));
        newEffect.GetComponent<SpecialEffect>().EffectType = Stats.DamageType;
        newEffect.transform.localScale = HeroSprite.transform.localScale;
        if (Stats.DamageType == 0)
        {
            sfx.PlaySwordSound();
        }
        else if(Stats.DamageType == 3)
        {
            sfx.PlayClubSound();
        }
        else if (Stats.DamageType == 2)
        {
            sfx.PlayBowSound();
        }
        else if (Stats.DamageType == 1)
        {
            sfx.PlayWandSound();
        }
        if (CurrentTarget == null)
        {
            return;
        }
        if (Stats.DamageType == CurrentTarget.GetComponent<Enemy>().DamageWeakness)
        {
            var dmg = Mathf.CeilToInt(Stats.Damage * 1.5f);
            CurrentTarget.GetComponent<Enemy>().TakeDamage(dmg);
            var newNumber = Instantiate(FloatingNumber, CurrentTarget.position, Quaternion.Euler(Vector3.forward));
            Color orangePlease = new Color(1, 0.5f, 0);
            newNumber.GetComponentInChildren<TextMeshProUGUI>().color = orangePlease;
            newNumber.GetComponentInChildren<TextMeshProUGUI>().text = dmg + "!";
        }
        else
        {
            var dmg = Stats.Damage;
            CurrentTarget.GetComponent<Enemy>().TakeDamage(dmg);
            var newNumber = Instantiate(FloatingNumber, CurrentTarget.position, Quaternion.Euler(Vector3.forward));
            newNumber.GetComponentInChildren<TextMeshProUGUI>().text = dmg.ToString();
        }

        step = 0;

        if (CurrentTarget.GetComponent<Enemy>().HP <= 0)
        {
            Instantiate(BonesPrefab, CurrentTarget.position + new Vector3(0,0,0.8f), Quaternion.Euler(90, 0, 0));
            var GoldFound = Mathf.CeilToInt(CurrentTarget.GetComponent<Enemy>().Gold * Stats.Discovery * DM.GoldBonus);
            var coinDrops = Mathf.CeilToInt(GoldFound / 4);
            coinDrops = Mathf.Clamp(coinDrops, 1, 20);
            for(var i = 0; i < coinDrops; i++)
            {
                var newCoin = Instantiate(CoinPrefab, CurrentTarget.position, Quaternion.Euler(90, 0, 0));
            }
            Stats.GoldHeld += GoldFound;
            if (CurrentTarget.GetComponent<Enemy>().XP + Stats.XP > Stats.MaxXP)
            {
                var newNumber = Instantiate(FloatingNumber, CurrentTarget.position - Vector3.forward * 0.5f, Quaternion.Euler(Vector3.forward));
                newNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL UP!";
                sfx.PlaySound(LevelUpSound);

            }
            else
            {
                var newNumber = Instantiate(FloatingNumber, CurrentTarget.position - Vector3.forward, Quaternion.Euler(Vector3.forward));
                newNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "+" + GoldFound +"G";
            }
            Stats.XP += CurrentTarget.GetComponent<Enemy>().XP;
            var lootChance = Random.Range(0, 21);
            if(lootChance <= Stats.LootFind)
            {
                Instantiate(ChestPrefab, CurrentTarget.position, Quaternion.Euler(90, 0, 0));
                sfx.PlaySound(ChestSound);
                Stats.LootHeld++;
                var chestLevel = Random.Range(DM.Level, DM.Level * 2);
                chestLevel = Mathf.RoundToInt(chestLevel);
                Stats.ChestLevels.Add(chestLevel);
                var newNumber = Instantiate(FloatingNumber, CurrentTarget.position - Vector3.forward * 1.5f, Quaternion.Euler(Vector3.forward));
                newNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                newNumber.GetComponentInChildren<TextMeshProUGUI>().text = "Loot Found!";
            }
            
            var e = CurrentTarget.GetComponent<Enemy>();

            e.Die();
            CurrentTarget = null;
            State = HeroState.Idle;
        }
    }

    public void TakeDamage(float i, Enemy e)
    {
        Camera.AddShake(0.5f);
        var damageEffect = Instantiate(EffectPrefab, transform.position + new Vector3(0, 0, 1), Quaternion.Euler(90, 0, 0));
        damageEffect.GetComponent<SpecialEffect>().EffectType = 4;
        damageEffect.transform.localScale = e.EnemySprite.transform.localScale;
        GetComponent<Flash>().FlashWhite();
        sfx.PlayDamageSound();
        if(Stats.HP > i)
        {
            Stats.HP -= i;
            var newNumber = Instantiate(FloatingNumber, transform.position, Quaternion.Euler(Vector3.forward));
            newNumber.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            newNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            if(CurrentTarget == null)
            {
                CurrentTarget = e.transform;
                State = HeroState.Attacking;
                step = UpdateTime;
            }
            if(Stats.ConsumableItem != null)
            {
                var con = Stats.ConsumableItem.GetComponent<Consumable>();
                if (con.Type == Consumable.ConsumableType.Potion)
                {
                    if(Stats.HP <= (Stats.MaxHP - con.Value))
                    {
                        var potionEffect = Instantiate(EffectPrefab, transform.position + new Vector3(0, 0, 1), Quaternion.Euler(90, 0, 0));
                        potionEffect.GetComponent<SpecialEffect>().EffectType = 5;
                        sfx.PlaySound(HealSound);
                        Debug.Log("Hero Healed");
                        Stats.HP += con.Value;
                        Destroy(Stats.ConsumableItem.gameObject);
                        Stats.ConsumableItem = null;
                        DM.ConsumableIcon.sprite = DM.HandSprite;
                        var healNumber = Instantiate(FloatingNumber, transform.position, Quaternion.Euler(Vector3.forward * 1));
                        healNumber.GetComponentInChildren<TextMeshProUGUI>().text = "+" + con.Value;
                        healNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                    }
                }
                
            }
        }
        else
        {
            if(Stats.ConsumableItem == null)
            {
                e.Hero = null;
                sfx.PlaySound(DeathSound);

                Die();
            }
            else
            {
                var con = Stats.ConsumableItem.GetComponent<Consumable>();
                if (con.Type == Consumable.ConsumableType.Portal)
                {
                    
                    var portalEffect = Instantiate(EffectPrefab, transform.position + new Vector3(0, 0, 1), Quaternion.Euler(90, 0, 0));
                    portalEffect.GetComponent<SpecialEffect>().EffectType = 6;
                    sfx.PlaySound(TeleportSound);
                    Destroy(Stats.ConsumableItem.gameObject);
                    Stats.ConsumableItem = null;
                    DM.ConsumableIcon.sprite = DM.HandSprite;
                    Waiting = true;
                    Active = false;
                    LevelCleared = false;
                    CurrentTarget = null;
                    Agent.isStopped = true;
                    DM.Running = false;
                    State = HeroState.Idle;
                    HeroSprite.enabled = false;
                    PortalOut = true;
                }
                else if(con.Type == Consumable.ConsumableType.Potion)
                {
                    Debug.Log("Hero Healed");
                    Stats.HP += con.Value;
                    Stats.HP = Mathf.Clamp(Stats.HP, 0, Stats.MaxHP);
                    Destroy(Stats.ConsumableItem.gameObject);
                    Stats.ConsumableItem = null;
                    DM.ConsumableIcon.sprite = DM.HandSprite;
                    var healNumber = Instantiate(FloatingNumber, transform.position, Quaternion.Euler(Vector3.forward * 1));
                    healNumber.GetComponentInChildren<TextMeshProUGUI>().text = "+" + con.Value;
                    healNumber.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    e.Hero = null;
                    Die();
                }
            }
            
        }
       

    }

    public void ExitPortal()
    {

        DungeonOverlay.Play("Dungeon End");
        Debug.Log("Hero Used Town Portal");
        Manager.ReturnHero();
        
        DM.DungeonCompleted = false;
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

    public void CompletedLevel()
    {
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

    public void ResetCamera()
    {
        Camera.transform.position = transform.position + new Vector3(0, 5, 0);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!PlayerControlled || !LevelCleared)
        {
            return;
        }
        Debug.Log(other.name);
        if(other.tag == "Exit")
        {
            CompletedLevel();
        }
    }




}
