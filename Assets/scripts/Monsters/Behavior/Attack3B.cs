using UnityEngine;
using System.Collections;

/**
 * Bearbot / dragon 
 */
public class Attack3B : MonoBehaviour 
{
	public GameObject BulletPrefab;
    public GameObject FireMobile;
	public Transform FireInitialPos;

	public void Attack(GameObject target)
	{
		FireBullet(target);
	}

	private void FireBullet(GameObject target)
	{
		GameObject bullet = Instantiate<GameObject>(BulletPrefab);
        GameObject fire = Instantiate<GameObject>(FireMobile);
		bullet.transform.position = FireInitialPos.position;
        fire.transform.position = FireInitialPos.position;
        fire.transform.parent = bullet.transform;
		bullet.transform.LookAt(target.transform.position);
	}
}
