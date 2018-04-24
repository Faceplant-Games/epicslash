using UnityEngine;
using System.Collections;

public class HellFireManager : MonoBehaviour 
{
	public ExplosiveWings[] Wings;
	public float RateOfSpawn = 0.1f;
	public float RateOfFire = 5f;
	private Transform[] SpawnPoints;
	public GameObject HellFireBulletPrefab;

	public bool Unleash = false;
	public AudioClip[] Explosions;

	public void PLayRandomSFXExplosion(Vector3 position)
	{
		//AudioSource.PlayClipAtPoint(Explosions[Random.Range(0, Explosions.Length)], position);
	}

	void Start()
	{
		SpawnPoints = transform.GetComponentsInChildren<Transform>();
	}

	void Update()
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
		foreach (ExplosiveWings i in Wings)
		{
			if (i != null)
			{
				i.ExplosionEnabled = true;
			}
		}
		for (int i = 1; i < SpawnPoints.Length; i++)
		{
//			GameObject bullet =  Instantiate(HellFireBulletPrefab, SpawnPoints[i].position, Quaternion.identity) as GameObject;
            GameObject bullet = Game.gameManager.GetBulletGenerator().PoolHellFireBullet.GetObject();
            bullet.transform.position = SpawnPoints[i].position;
            bullet.transform.LookAt(Camera.main.transform.position);
            bullet.SetActive(true);
			yield return new WaitForSeconds(RateOfSpawn);
		}
		foreach (ExplosiveWings i in Wings)
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
