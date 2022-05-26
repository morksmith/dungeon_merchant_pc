using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGenerator : MonoBehaviour
{
    
    public List<string> KnightFirstNames;
    public List<string> KnightLastNames;
    public List<string> MageFirstNames;
    public List<string> MageLastNames;
    public List<string> RogueFirstNames;
    public List<string> RogueLastNames;
    public List<string> WarriorFirstNames;
    public List<string> WarriorLastNames;
    

    public List<GameObject> HeroPrefabs;
    public Transform HeroParent;
    public HireMenu HireScreen;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CreateHero();
        }
    }
    private void Start()
    {

    }
    public void CreateHero()
    {
        var pick = Random.Range(0, HeroPrefabs.Count);
        var newHero = Instantiate(HeroPrefabs[pick], HeroParent.position, HeroParent.rotation, HeroParent);
        if(pick == 0)
        {
            var fn = Random.Range(0, KnightFirstNames.Count);
            var ln = Random.Range(0, KnightLastNames.Count);
            newHero.name = KnightFirstNames[fn] + KnightLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;
        }
        if(pick == 1)
        {
            var fn = Random.Range(0, MageFirstNames.Count);
            var ln = Random.Range(0, MageLastNames.Count);
            newHero.name = MageFirstNames[fn] + MageLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;

        }
        if (pick == 2)
        {
            var fn = Random.Range(0, RogueFirstNames.Count);
            var ln = Random.Range(0, RogueLastNames.Count);
            newHero.name = RogueFirstNames[fn] + RogueLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;

        }
        if (pick == 3)
        {
            var fn = Random.Range(0, WarriorFirstNames.Count);
            var ln = Random.Range(0, WarriorLastNames.Count);
            newHero.name = WarriorFirstNames[fn] + WarriorLastNames[ln];
            newHero.GetComponent<Stats>().HeroName = newHero.name;

        }

        newHero.GetComponent<Stats>().Level = Random.Range(1, 3);
        HireScreen.UpdateHeroInfo(newHero.gameObject.transform);
        newHero.gameObject.SetActive(false);

    }
}
