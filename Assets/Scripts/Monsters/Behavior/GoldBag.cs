using UnityEngine;
using System.Collections;

public class GoldBag : MonoBehaviour 
{
	public float GoldValue;

	private float timeEllapsed = 0;

	void Start() {
	}

	void FixedUpdate() {
		timeEllapsed += Time.fixedDeltaTime;

		if (timeEllapsed >= 5f)
        {
            ObjectPool pool = null;

            switch (gameObject.name)
            {
                case CoinGenerator.SMALLCOIN:
                    pool = gameObject.GetComponent<GameManager>().getCoinGenerator().PoolSmallCoin;
                    break;
                case CoinGenerator.BIGGERCOIN:
                    pool = gameObject.GetComponent<GameManager>().getCoinGenerator().PoolBiggerCoin;
                    break;

                case CoinGenerator.BIGCOIN:
                    pool = gameObject.GetComponent<GameManager>().getCoinGenerator().PoolBigCoin;
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
