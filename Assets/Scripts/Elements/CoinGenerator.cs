using UnityEngine;
using System.Collections;
using System;


public class CoinGenerator : MonoBehaviour
{
    public const string SMALLCOIN = "SmallCoin";
    public const string BIGGERCOIN = "BiggerCoin";
    public const string BIGCOIN = "BigCoin";


    public ObjectPool PoolSmallCoin { get; set; }
    public ObjectPool PoolBiggerCoin { get; set; }
    public ObjectPool PoolBigCoin { get; set; }
    
    void Start()
    {
        PoolSmallCoin = gameObject.AddComponent<ObjectPool>();
        PoolBiggerCoin = gameObject.AddComponent<ObjectPool>();
        PoolBigCoin = gameObject.AddComponent<ObjectPool>();
        PoolSmallCoin.Initialize(20, (GameObject)Resources.Load(SMALLCOIN, typeof(GameObject)));
        PoolBiggerCoin.Initialize(20, (GameObject)Resources.Load(BIGGERCOIN, typeof(GameObject)));
        PoolBigCoin.Initialize(20, (GameObject)Resources.Load(BIGCOIN, typeof(GameObject)));
    }


    private ArrayList GenerateCoins(int experience)
    {
        ArrayList coins = new ArrayList();
        int compteur = 1;
        do
        {
            if (compteur > 10000)
            {
                coins.Add(BIGCOIN);
            }
            else if (compteur > 100)
            {
                coins.Add(BIGGERCOIN);
            }
            else
            {
                coins.Add(SMALLCOIN);
            }

        } while (compteur < experience);
        return coins;
    }


    public void SpawnGold(int experience, Vector3 position, Quaternion rotation)
    {
        ArrayList coinsToSpawn = GenerateCoins(experience);
        foreach (String coin in coinsToSpawn)
        {
            GameObject objectToSpawn = getCoinObjectFromName(coin);
            Vector3 random1 = new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f));
            Vector3 pos = position + random1;
            objectToSpawn.transform.position = pos;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
        }
    }


    private GameObject getCoinObjectFromName(String name)
    {
        switch (name)
        {
            case SMALLCOIN:
                return PoolSmallCoin.GetObject();
            case BIGGERCOIN:
                return PoolBigCoin.GetObject();
            case BIGCOIN:
                return PoolBigCoin.GetObject();
            default:
                return PoolSmallCoin.GetObject();
        }

    }

}