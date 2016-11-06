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
		for (int i = 0; i < SpawnPoints.Length; i++)
		{
			GameObject bullet =  Instantiate(HellFireBulletPrefab, SpawnPoints[i].position, Quaternion.identity) as GameObject;
			bullet.transform.LookAt(Camera.main.transform.position);
			yield return new WaitForSeconds(RateOfSpawn);
		}
		foreach (ExplosiveWings i in Wings)
		{
			i.ExplosionEnabled = false;
		}
		yield return new WaitForSeconds(RateOfFire);
		StartCoroutine(UnleashHellFire());
	}


}
