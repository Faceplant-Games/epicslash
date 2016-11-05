using UnityEngine;
using System.Collections;

public class Attack3B : MonoBehaviour 
{
	public GameObject BulletPrefab;
	public Transform FireInitialPos;

	public void Attack(GameObject target)
	{
        print("je suis un putain de dragon et je tire");
		FireBullet(target);
	}

	private void FireBullet(GameObject target)
	{
		GameObject bullet = Instantiate<GameObject>(BulletPrefab);
		bullet.transform.position = FireInitialPos.position;
		bullet.transform.LookAt(target.transform.position);
	}
}
