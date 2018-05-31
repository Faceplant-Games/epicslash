using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(GameManager)]
public class GoldBag : MonoBehaviour 
{
	public float GoldValue;
    private GameManager gameManager;

	private float timeEllapsed = 0;

	void Start() {
        gameManager = Game.GameManager;
    }

    void FixedUpdate() {
		timeEllapsed += Time.fixedDeltaTime;

		if (timeEllapsed >= 5f)
        {
            ObjectPool pool = null;

            switch (gameObject.tag)
            {
                case CoinGenerator.SmallCoin:
                    pool = gameManager.GetCoinGenerator().PoolSmallCoin;
                    break;
                case CoinGenerator.BiggerCoin:
                    pool = gameManager.GetCoinGenerator().PoolBiggerCoin;
                    break;

                case CoinGenerator.BigCoin:
                    pool = gameManager.GetCoinGenerator().PoolBigCoin;
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
