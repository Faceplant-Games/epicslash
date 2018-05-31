using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    private const string EnemyBullet = "BulletPrefabM3";
    private const string HellfireBullet = "HellFireBullet";

    public ObjectPool PoolEnemyBullet { get; set; }
    public ObjectPool PoolHellFireBullet { get; set; }

    void Start()
    {
        PoolEnemyBullet = gameObject.AddComponent<ObjectPool>();
        PoolEnemyBullet.Initialize(30, Resources.Load<GameObject>(EnemyBullet));
    }

    public void InitializeHellFireBullet()
    {
        PoolHellFireBullet = gameObject.AddComponent<ObjectPool>();
        PoolHellFireBullet.Initialize(100, Resources.Load<GameObject>(HellfireBullet));
    }
}