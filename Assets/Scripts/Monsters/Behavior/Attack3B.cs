using UnityEngine;

/**
 * Bearbot and Dragon attack
 */
public class Attack3B : MonoBehaviour 
{
	public Transform FireInitialPos;
    public int Damage = 5;

	public void Attack(GameObject target)
	{
		FireBullet(target);
	}

	private void FireBullet(GameObject target)
	{
		var bullet = Game.GameManager.GetBulletGenerator().PoolEnemyBullet.GetObject();
		bullet.transform.position = FireInitialPos.position;
		bullet.transform.LookAt(target.transform.position);
        bullet.GetComponent<Ennemi3BulletB>().Damage = Damage;
        bullet.SetActive(true);
	}
}
