using System.Collections;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public const string SmallCoin = "SmallCoin";
    public const string BiggerCoin = "BiggerCoin";
    public const string BigCoin = "BigCoin";

    public ObjectPool PoolSmallCoin { get; private set; }
    public ObjectPool PoolBiggerCoin { get; private set; }
    public ObjectPool PoolBigCoin { get; private set; }

    private void Start()
    {
        PoolSmallCoin = gameObject.AddComponent<ObjectPool>();
        PoolBiggerCoin = gameObject.AddComponent<ObjectPool>();
        PoolBigCoin = gameObject.AddComponent<ObjectPool>();
        PoolSmallCoin.Initialize(20, Resources.Load<GameObject>(SmallCoin));
        PoolBiggerCoin.Initialize(20, Resources.Load<GameObject>(BiggerCoin));
        PoolBigCoin.Initialize(20, Resources.Load<GameObject>(BigCoin));
    }


    private static ArrayList GenerateCoins(int experience)
    {
        var coins = new ArrayList();
        var compteur = 1;
        do
        {
            if (compteur > 10000)
            {
                coins.Add(BigCoin);
            }
            else if (compteur > 100)
            {
                coins.Add(BiggerCoin);
            }
            else
            {
                coins.Add(SmallCoin);
            }
            compteur = compteur * 10;
        } while (compteur < experience);
        return coins;
    }


    public void SpawnGold(int experience, Vector3 position, Quaternion rotation)
    {
        var coinsToSpawn = GenerateCoins(experience);
        foreach (string coin in coinsToSpawn)
        {
            var objectToSpawn = GetCoinObjectFromName(coin);
            var random1 = new Vector3(Random.Range(-2f, 2f), Random.Range(2.5f, 5f), Random.Range(-2f, 2f));
            var pos = position + random1;
            objectToSpawn.transform.position = pos;
            objectToSpawn.transform.rotation = rotation;
            var rbd = objectToSpawn.GetComponent<Rigidbody>();
            rbd.velocity = new Vector3(Random.Range(-2, 2), Random.Range(1, 2), Random.Range(-2, 2));
            rbd.angularVelocity = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
            objectToSpawn.SetActive(true);
        }
    }


    private GameObject GetCoinObjectFromName(string coinName)
    {
        switch (coinName)
        {
            case SmallCoin:
                return PoolSmallCoin.GetObject();
            case BiggerCoin:
                return PoolBigCoin.GetObject();
            case BigCoin:
                return PoolBigCoin.GetObject();
            default:
                return PoolSmallCoin.GetObject();
        }
    }
}