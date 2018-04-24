using UnityEngine;
using System.Collections;


public class BulletGenerator : MonoBehaviour
{
    public const string ENEMY_BULLET = "BulletPrefabM3";
    public const string HELLFIRE_BULLET = "HellFireBullet";

    public ObjectPool PoolEnemyBullet { get; set; }
    public ObjectPool PoolHellFireBullet { get; set; }

    void Start()
    {
        PoolEnemyBullet = gameObject.AddComponent<ObjectPool>();
        PoolEnemyBullet.Initialize(30, Resources.Load<GameObject>(ENEMY_BULLET));
    }

    public void InitializeHellFireBullet()
    {
        PoolHellFireBullet = gameObject.AddComponent<ObjectPool>();
        PoolHellFireBullet.Initialize(100, Resources.Load<GameObject>(HELLFIRE_BULLET));
    }
}