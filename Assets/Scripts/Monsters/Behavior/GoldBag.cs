using UnityEngine;

//[RequireComponent(typeof(GameManager)]
public class GoldBag : MonoBehaviour 
{
	public float GoldValue;

	private float _timeEllapsed;

    private void FixedUpdate() {
		_timeEllapsed += Time.fixedDeltaTime;

		if (_timeEllapsed >= 5f)
        {
            ObjectPool pool = null;

            switch (gameObject.tag)
            {
                case CoinGenerator.SmallCoin:
                    pool = Game.GameManager.GetCoinGenerator().PoolSmallCoin;
                    break;
                case CoinGenerator.BiggerCoin:
                    pool = Game.GameManager.GetCoinGenerator().PoolBiggerCoin;
                    break;
                case CoinGenerator.BigCoin:
                    pool = Game.GameManager.GetCoinGenerator().PoolBigCoin;
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
