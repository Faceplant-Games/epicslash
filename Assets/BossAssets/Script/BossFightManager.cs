using UnityEngine;
using System.Collections;

public class BossFightManager : MonoBehaviour 
{
	public GameObject Boss;
	private WaitForEndOfFrame waitForEndOfFrame =  new WaitForEndOfFrame();

	public Transform[] BossPositions;
	public MeshRenderer[] BossEyes;
	public Material RedEyeMaterial;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(StartBossFight());
	}

	IEnumerator StartBossFight()
	{
		yield return new WaitForSeconds(3);
		StartCoroutine(MoveBoss(BossPositions[1].position, 15));
		yield return new WaitForSeconds(16);
		TurnOnEyes();
		yield return new WaitForSeconds(1);
		StartCoroutine(MoveBoss(BossPositions[2].position, 8));
		yield return new WaitForSeconds(8.5f);
		StartCoroutine(RotateBoss(BossPositions[3].rotation, 5));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void TurnOnEyes()
	{
		foreach (MeshRenderer i in BossEyes)
		{
			i.material = RedEyeMaterial;
		}
	}

	public IEnumerator MoveBoss(Vector3 position, float duration)
	{
		float timer = 0;
		Vector3 InitialPos = Boss.transform.position;
		while (timer < duration)
		{
			yield return waitForEndOfFrame;
			timer += Time.deltaTime;
			Boss.transform.position = InitialPos + (position - InitialPos) * (timer/duration);
		}
		Boss.transform.position = position;
		yield return null;
	}

	public IEnumerator RotateBoss(Quaternion rotation, float duration)
	{
		float timer = 0;
		Quaternion InitialRot = Boss.transform.rotation;
		while (timer < duration)
		{
			yield return waitForEndOfFrame;
			timer += Time.deltaTime;
			Boss.transform.rotation = Quaternion.Lerp(InitialRot, rotation, timer/duration);
		}
		Boss.transform.rotation = rotation;
		yield return null;
	}
}
