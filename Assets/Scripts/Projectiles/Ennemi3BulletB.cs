using UnityEngine;
using System.Collections;

public class Ennemi3BulletB : MonoBehaviour 
{
	private float timer = 0;
	private float lifeTime = 60;
	public float speed = 2f;
    public int damage = 5;

    // Update is called once per frame
    void FixedUpdate () {
		float distanceThisFrame = speed * Time.fixedDeltaTime;
		RaycastHit hit = new RaycastHit ();

		// At each frame, we cast a ray forward from where we are to where we will be next frame
		if (Physics.Raycast (transform.position, transform.forward, out hit, distanceThisFrame)) 
		{
			if (hit.transform.gameObject.tag == "MainCamera") 
			{
                Game.GameManager.LoseExperience(damage);
            }
            DestroyBullet();
            return;
        } 
		else 
		{
			transform.position += transform.forward * distanceThisFrame;
		}

		timer += Time.fixedDeltaTime;
		if (timer > lifeTime) {
            DestroyBullet();
            return;
		}
	}

    void DestroyBullet()
    {
        Game.GameManager.GetBulletGenerator().PoolEnemyBullet.DestroyObjectPool(gameObject);
    }
}
