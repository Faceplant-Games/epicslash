using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class MonsterGenerator : MonoBehaviour
{
    private readonly Dictionary<string, ObjectPool> _monstersPools = new Dictionary<string, ObjectPool>();

    private ObjectPool SpiderPool { get; set; }
    private ObjectPool BunnyPool { get; set; }
    private ObjectPool BearbotPool { get; set; }
    private ObjectPool DragonPool { get; set; }

    private void Start()
    {
        SpiderPool = gameObject.AddComponent<ObjectPool>();
        BunnyPool = gameObject.AddComponent<ObjectPool>();
        BearbotPool = gameObject.AddComponent<ObjectPool>();
        DragonPool = gameObject.AddComponent<ObjectPool>();
        _monstersPools.Add("Spider", SpiderPool);
        _monstersPools.Add("Bunny", BunnyPool);
        _monstersPools.Add("Bearbot", BearbotPool);
        _monstersPools.Add("Dragon", DragonPool);
        var gameManager = GetComponent<GameManager>();
        foreach (var data in gameManager.Data.stages[Game.GetCurrentStage()].monsters)
        {
            ObjectPool currentPool;
            if (_monstersPools.ContainsKey(data.name) && _monstersPools.TryGetValue(data.name, out currentPool))
            {
                currentPool.Initialize((int) (1 / data.spawnPeriod * 60), Resources.Load<GameObject>("Monsters/"+data.name));
            }
        }
    }

    internal void DestroyObjectPool(GameObject objectToDestroy)
    {
        ObjectPool pool;
        if (_monstersPools.TryGetValue(objectToDestroy.name, out pool)) {
            pool.DestroyObjectPool(objectToDestroy);
        }
        else
        {
            objectToDestroy.SetActive(false);
            Destroy(objectToDestroy);
        }
    }

    public GameObject GetMonsterFromName( string monsterName )
    {
        ObjectPool pool;
        if (_monstersPools.TryGetValue(monsterName, out pool))
        {
            var monster = pool.GetObject();
            monster.SetActive(true);
            return monster;
        }
        return SpiderPool.GetObject();
    }
    
}
