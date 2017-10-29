using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(GameManager)]
public class GoldBag : MonoBehaviour 
{
	public float GoldValue;
    private GameManager gameManager;

	private float timeEllapsed = 0;

	void Start() {
        gameManager = GameObject.FindObjectsOfType(typeof(GameManager))[0] as GameManager;
    }

    void FixedUpdate() {
		timeEllapsed += Time.fixedDeltaTime;

		if (timeEllapsed >= 5f)
        {
            ObjectPool pool = null;

            switch (gameObject.tag)
            {
                case CoinGenerator.SMALLCOIN:
                    pool = gameManager.getCoinGenerator().PoolSmallCoin;
                    break;
                case CoinGenerator.BIGGERCOIN:
                    pool = gameManager.getCoinGenerator().PoolBiggerCoin;
                    break;

                case CoinGenerator.BIGCOIN:
                    pool = gameManager.getCoinGenerator().PoolBigCoin;
                    break;
            }
            if (pool != null)
            {
                pool.DestroyObjectPool(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

		}
	}
}
