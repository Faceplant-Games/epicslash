using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFightManager : MonoBehaviour 
{
	public GameObject Boss;
	private readonly WaitForEndOfFrame _waitForEndOfFrame =  new WaitForEndOfFrame();

	public Transform[] BossPositions;
	public MeshRenderer[] BossEyes;
	public MeshRenderer[] BossWings;
	public Material RedEyeMaterial;
	public Material WhiteEyeMaterial;
	public Material BlueWingsMaterial;

	private int _eyesBrokenStage1;
	public int EyesBrokenStage1
	{
		get
		{
			return _eyesBrokenStage1;
		}
		set
		{
			_eyesBrokenStage1 = value;
			if (_eyesBrokenStage1 == 2)
			{
				StartCoroutine(BossPhase2());
			}
		}
	}

	private int _wingsBrokenStage2;
	public int WingsBrokenStage2
	{
		get
		{
			return _wingsBrokenStage2;
		}
		set
		{
			_wingsBrokenStage2 = value;
			if (_wingsBrokenStage2 == 2)
			{
				StartCoroutine(BossPhase3());
			}
		}
	}

	// Use this for initialization
	private void Start () 
	{
		Boss.transform.position = BossPositions[0].position;
        Game.GameManager.GetBulletGenerator().InitializeHellFireBullet();
		StartCoroutine(StartBossFight());
	}

	private IEnumerator StartBossFight()
	{
		yield return new WaitForSeconds(3);
		StartCoroutine(MoveBoss(BossPositions[1].position, 15));
		yield return new WaitForSeconds(16);
		TurnOnEyesRed();
		yield return new WaitForSeconds(1);
		StartCoroutine(MoveBoss(BossPositions[2].position, 8));
		yield return new WaitForSeconds(8.5f);
		TurnOnEyesWhite();
		StartCoroutine(RotateBoss(BossPositions[3].rotation, 5));
	}

	private void Update()
	{
        if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(BossPhase2());
        }
        if (Input.GetKey(KeyCode.E))
        {
            StartCoroutine(BossPhase3());
        }

        if (Input.GetKey(KeyCode.B))
        {
            EndGame();
        }

    }

    public void EndGame()
	{
		Destroy(Boss);
		SceneManager.LoadScene("Credits");
		//LoadCredits
	}

	private IEnumerator BossPhase2()
	{
		StartCoroutine(RotateBoss(BossPositions[2].rotation, 4));
		yield return new WaitForSeconds(5f);
		StartCoroutine(MoveBoss(BossPositions[4].position, 8));
		yield return new WaitForSeconds(8f);
		FindObjectOfType<HellFireManager>().Unleash = true;
		StartCoroutine(FindObjectOfType<HellFireManager>().UnleashHellFire());
	}

	private IEnumerator BossPhase3()
	{
        FindObjectOfType<HellFireManager>().Unleash = false;
        StartCoroutine(RotateBoss(BossPositions[5].rotation, 6));
		StartCoroutine(MoveBoss(BossPositions[5].position, 6));
		yield return null;
	}

	private void TurnWingsWhite() // TODO dead code: keep it or delete it
	{
		foreach (var i in BossWings)
		{
			i.material = RedEyeMaterial;
		}
	}

	private void TurnOnEyesRed()
	{
		foreach (var i in BossEyes)
		{
			i.material = RedEyeMaterial;
		}
	}

	private void TurnOnEyesWhite()
	{
		foreach (var i in BossEyes)
		{
			i.material = WhiteEyeMaterial;
			i.GetComponent<ExplosiveSurface>().ExplosionEnabled = true;
		}
	}

	private IEnumerator MoveBoss(Vector3 position, float duration)
	{
		float timer = 0;
		var initialPos = Boss.transform.position;
		while (timer < duration)
		{
			yield return _waitForEndOfFrame;
			timer += Time.deltaTime;
			Boss.transform.position = initialPos + (position - initialPos) * (timer/duration);
		}
		Boss.transform.position = position;
		yield return null;
	}

	private IEnumerator RotateBoss(Quaternion rotation, float duration)
	{
		float timer = 0;
		var initialRot = Boss.transform.rotation;
		while (timer < duration)
		{
			yield return _waitForEndOfFrame;
			timer += Time.deltaTime;
			Boss.transform.rotation = Quaternion.Lerp(initialRot, rotation, timer/duration);
		}
		Boss.transform.rotation = rotation;
		yield return null;
	}
}
