using UnityEngine;
using System.Collections;

public class BossFightManager : MonoBehaviour 
{
	public GameObject Boss;
	private WaitForEndOfFrame waitForEndOfFrame =  new WaitForEndOfFrame();

	public Transform[] BossPositions;
	public MeshRenderer[] BossEyes;
	public Material RedEyeMaterial;
	public Material WhiteEyeMaterial;

	private int _eyesBrokenStage1;
	public int EyesBrokenStage1
	{
		get
		{
			if (_eyesBrokenStage1 == 2)
			{
				StartCoroutine(BossPhase2());
			}
			return _eyesBrokenStage1;
		}
		set
		{
			_eyesBrokenStage1 = value;
		}
	}

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
		TurnOnEyesRED();
		yield return new WaitForSeconds(1);
		StartCoroutine(MoveBoss(BossPositions[2].position, 8));
		yield return new WaitForSeconds(8.5f);
		TurnOnEyesWhite();
		StartCoroutine(RotateBoss(BossPositions[3].rotation, 5));
	}

	public IEnumerator BossPhase2()
	{
		StartCoroutine(RotateBoss(BossPositions[2].rotation, 4));
		yield return new WaitForSeconds(5f);
	}

	private void TurnOnEyesRED()
	{
		foreach (MeshRenderer i in BossEyes)
		{
			i.material = RedEyeMaterial;
		}
	}

	private void TurnOnEyesWhite()
	{
		foreach (MeshRenderer i in BossEyes)
		{
			i.material = WhiteEyeMaterial;
			i.GetComponent<ExplosiveSurface>().ExplosionEnabled = true;
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
