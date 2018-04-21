using UnityEngine;
using System.Collections;

/**
 * Bearbot / dragon 
 */
public class Attack3B : MonoBehaviour 
{
	public GameObject BulletPrefab;
	public Transform FireInitialPos;

	public void Attack(GameObject target)
	{
		FireBullet(target);
	}

	private void FireBullet(GameObject target)
	{
		GameObject bullet = Instantiate<GameObject>(BulletPrefab);
		bullet.transform.position = FireInitialPos.position + new Vector3(0, 2.8f, 0);
		bullet.transform.LookAt(target.transform.position);
	}
}
