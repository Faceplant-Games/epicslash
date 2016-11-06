using UnityEngine;
using System.Collections;

public class UltimateExplodingSurface : MonoBehaviour 
{
	public GameObject ExplosionFX;
	private int HP = 20;

	public void TriggerExplosion(Vector3 position)
	{
		GameObject FX = Instantiate(ExplosionFX, position, Quaternion.identity) as GameObject;
		HP --;
		if (HP < 0)
		{
			//End The Game
		}

	}
}
