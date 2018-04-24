using UnityEngine;
using System.Collections;

/**
 * Bearbot and Dragon attack
 */
public class Attack3B : MonoBehaviour 
{
	public Transform FireInitialPos;
    public int damage = 5;

	public void Attack(GameObject target)
	{
		FireBullet(target);
	}

	private void FireBullet(GameObject target)
	{
		GameObject bullet = Game.gameManager.GetBulletGenerator().PoolEnemyBullet.GetObject();
		bullet.transform.position = FireInitialPos.position;
		bullet.transform.LookAt(target.transform.position);
        bullet.GetComponent<Ennemi3BulletB>().damage = damage;
        bullet.SetActive(true);
	}
}
