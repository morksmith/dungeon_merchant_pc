using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public DungeonManager Dungeon;
    public List<GameObject> BeastEnemies;
    public List<GameObject> GhostEnemies;
    public List<GameObject> SkeletonEnemies;
    public List<GameObject> DemonEnemies;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(int t, int l, bool survival)
    {
        if(t == 0)
        {
            var pick = Random.Range(0, BeastEnemies.Count);
            var newEnemy = Instantiate(BeastEnemies[pick], transform.position, transform.rotation);
            var info = newEnemy.GetComponent<Enemy>();
            info.Level = l;
            if (survival)
            {
                newEnemy.GetComponent<NavMeshAgent>().speed = 2;
                info.Range = 2;
            }
        }
        else if (t == 1)
        {
            var pick = Random.Range(0, GhostEnemies.Count);
            var newEnemy = Instantiate(GhostEnemies[pick], transform.position, transform.rotation);
            var info = newEnemy.GetComponent<Enemy>();
            info.Level = l;
            if (survival)
            {
                newEnemy.GetComponent<NavMeshAgent>().speed = 2;
                info.Range = 2;
            }
        }
        else if (t == 2)
        {
            var pick = Random.Range(0, DemonEnemies.Count);
            var newEnemy = Instantiate(DemonEnemies[pick], transform.position, transform.rotation);
            var info = newEnemy.GetComponent<Enemy>();
            info.Level = l;
            if (survival)
            {
                newEnemy.GetComponent<NavMeshAgent>().speed = 2;
                info.Range = 2;

            }
        }
        else if (t == 3)
        {
            var pick = Random.Range(0, SkeletonEnemies.Count);
            var newEnemy = Instantiate(SkeletonEnemies[pick], transform.position, transform.rotation);
            var info = newEnemy.GetComponent<Enemy>();
            info.Level = l;
            if (survival)
            {
                newEnemy.GetComponent<NavMeshAgent>().speed = 2;
                info.Range = 2;

            }
        }

    }
}
