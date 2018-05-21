using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(GameManager))]
public class MonsterGenerator : MonoBehaviour
{
    private Dictionary<string, ObjectPool> monstersPools = new Dictionary<string, ObjectPool>();

    private ObjectPool SpiderPool { get; set; }
    private ObjectPool BunnyPool { get; set; }
    private ObjectPool BearbotPool { get; set; }
    private ObjectPool DragonPool { get; set; }

    void Start()
    {
        SpiderPool = gameObject.AddComponent<ObjectPool>();
        BunnyPool = gameObject.AddComponent<ObjectPool>();
        BearbotPool = gameObject.AddComponent<ObjectPool>();
        DragonPool = gameObject.AddComponent<ObjectPool>();
        monstersPools.Add("Spider", SpiderPool);
        monstersPools.Add("Bunny", BunnyPool);
        monstersPools.Add("Bearbot", BearbotPool);
        monstersPools.Add("Dragon", DragonPool);
        GameManager gameManager = GetComponent<GameManager>();
        ObjectPool currentPool;
        foreach (GameManager.GameData.Stage.MonsterData data in gameManager.gameData.stages[Game.GetCurrentStage()].monsters)
        {
            if (monstersPools.ContainsKey(data.name) && monstersPools.TryGetValue(data.name, out currentPool))
            {
                currentPool.Initialize((int) (1 / data.spawnPeriod * 60), Resources.Load<GameObject>(data.name));
            }
        }
    }

    internal void DestroyObjectPool(GameObject gameObject)
    {
        ObjectPool pool;
        if (monstersPools.TryGetValue((gameObject.GetComponent(typeof(AbstractMonster)) as AbstractMonster).Name, out pool)) {
            pool.DestroyObjectPool(gameObject);
        }
    }

    public GameObject GetMonsterFromName( string name )
    {
        GameObject monster;
        ObjectPool pool;
        if (monstersPools.TryGetValue(name, out pool))
        {
            monster = pool.GetObject();
            monster.SetActive(true);
            return monster;
        }
        return SpiderPool.GetObject();
    }
    
}
