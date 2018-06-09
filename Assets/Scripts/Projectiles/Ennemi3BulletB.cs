using UnityEngine;

public class Ennemi3BulletB : MonoBehaviour 
{
	private float _timer;
	private const float LifeTime = 60;
	private float Speed = 3.5f;
    public int Damage = 5;

    // Update is called once per frame
	private void FixedUpdate () {
		var distanceThisFrame = Speed * Time.fixedDeltaTime;
		var hit = new RaycastHit ();

		// At each frame, we cast a ray forward from where we are to where we will be next frame
		if (Physics.Raycast (transform.position, transform.forward, out hit, distanceThisFrame)) 
		{
			if (hit.transform.gameObject.CompareTag("MainCamera")) 
			{
                Game.GameManager.LoseExperience(Damage);
            }
            DestroyBullet();
            return;
        }

		transform.position += transform.forward * distanceThisFrame;

		_timer += Time.fixedDeltaTime;
		if (_timer > LifeTime) {
            DestroyBullet();
		}
	}

	private void DestroyBullet()
    {
        Game.GameManager.GetBulletGenerator().PoolEnemyBullet.DestroyObjectPool(gameObject);
    }
}
