using UnityEngine;

public class HellFireBullet : MonoBehaviour 
{
    private float _timer;
    private const float LifeTime = 60;
    public float speed = 2f;

    // Update is called once per frame
    private void FixedUpdate()
    {
        var distanceThisFrame = speed * Time.fixedDeltaTime;
        RaycastHit hit;

        // At each frame, we cast a ray forward from where we are to where we will be next frame
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceThisFrame))
        {
            if (hit.transform.gameObject.CompareTag("MainCamera"))
            {
                Game.GameManager.LoseExperience(5000);
            }
            DestroyBullet();
            return;
        }

        transform.position += transform.forward * distanceThisFrame;

        _timer += Time.fixedDeltaTime;
        if (_timer > LifeTime)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Game.GameManager.GetBulletGenerator().PoolHellFireBullet.DestroyObjectPool(gameObject);
    }
}
