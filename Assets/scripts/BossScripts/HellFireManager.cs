using System.Collections;
using UnityEngine;

public class HellFireManager : MonoBehaviour 
{
	public ExplosiveWings[] Wings;
	public float RateOfSpawn = 0.1f;
	public float RateOfFire = 5f;
	private Transform[] _spawnPoints;

	public bool Unleash = false;
	public AudioClip[] Explosions;

	public static void PlayRandomSfxExplosion(Vector3 position) // TODO Dead code: keep it or delete it
	{
		//AudioSource.PlayClipAtPoint(Explosions[Random.Range(0, Explosions.Length)], position);
	}

	private void Start()
	{
		_spawnPoints = transform.GetComponentsInChildren<Transform>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			Unleash = true;
			StartCoroutine(UnleashHellFire());
		}
	}

	public IEnumerator UnleashHellFire()
	{
		if (Unleash == false)
		{
			yield break;
		}
		foreach (var i in Wings)
		{
			if (i != null)
			{
				i.ExplosionEnabled = true;
			}
		}
		for (var i = 1; i < _spawnPoints.Length; i++)
		{
            var bullet = Game.GameManager.GetBulletGenerator().PoolHellFireBullet.GetObject();
            bullet.transform.position = _spawnPoints[i].position;
            bullet.transform.LookAt(Camera.main.transform.position);
            bullet.SetActive(true);
			yield return new WaitForSeconds(RateOfSpawn);
		}
		foreach (var i in Wings)
		{
			if (i != null)
			{
				i.ExplosionEnabled = false;
			}
		}
		yield return new WaitForSeconds(RateOfFire);
		StartCoroutine(UnleashHellFire());
	}


}
