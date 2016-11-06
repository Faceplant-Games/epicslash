using UnityEngine;
using System.Collections;

public class UltimateExplodingSurface : MonoBehaviour 
{
	public GameObject ExplosionFX;
	private int HP = 15;

	public void TriggerExplosion(Vector3 position)
	{
		GameObject FX = Instantiate(ExplosionFX, position, Quaternion.identity) as GameObject;
		HP --;
        FindObjectOfType<HellFireManager>().PLayRandomSFXExplosion(position);
        if (HP < 0)
		{
			FindObjectOfType<BossFightManager>().EndGame();
            Debug.Log("End Of The Game");
		}
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<WeaponB>() != null)
        {
            TriggerExplosion(transform.position);
        }
    }
}
