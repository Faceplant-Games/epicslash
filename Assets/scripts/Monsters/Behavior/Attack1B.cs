using UnityEngine;
using System.Collections;

public class Attack1B : MonoBehaviour 
{
	private float _facePlayerSpeed;
	private float _attackRate = 5f;
	private float _attackPendingDuration = 2f;
	private float _attackDistance = 2f;
	private float _attackTravelDuration = 2f;

	private IEnumerator _attackCoroutine;
	private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

	// Use this for initialization
	void Start () 
	{
		_facePlayerSpeed = StatManager.Attack1B_facePlayerSpeed;
	}

	public void Attack(GameObject player)
	{
		if (_attackCoroutine != null)
		{
			StopCoroutine(_attackCoroutine);
		}
		_attackCoroutine = AttackCoroutine(player);
		StartCoroutine(_attackCoroutine);
	}

	private void DisplayAttackSign()
	{
		Debug.Log(gameObject.name +  " is about to attack");
	}

	private void DisplayAttackTrajectory()
	{
		//Display Trajectory
	}

	private IEnumerator FaceObject(GameObject target, float duration)
	{
		Vector3 direction = target.transform.position - transform.position;
		Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
		float timer = 0;
		Quaternion initialRotation = transform.rotation;
		while(timer < duration)
		{
			yield return _waitForEndOfFrame;
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Lerp(initialRotation, toRotation, timer/duration);
		}
		transform.rotation = toRotation;
		yield return null;
	}

	private IEnumerator AttackMove()
	{
		float t = 0;
		Vector3 InitialPosition = transform.position;
		Vector3 FinalPosition = transform.position + new Vector3(0, 0, _attackDistance);
		while (t < _attackTravelDuration)
		{
			yield return _waitForEndOfFrame;
			t += Time.deltaTime;
			transform.position =  InitialPosition + (FinalPosition - InitialPosition) * t/_attackTravelDuration ;//à changer en fonction du type d'animation sur le perso
		}
	}

	private IEnumerator AttackCoroutine(GameObject player)
	{
		StartCoroutine(FaceObject(player, 0.5f));
		yield return new WaitForSeconds(1f);
		DisplayAttackSign();
		DisplayAttackTrajectory();
		yield return new WaitForSeconds(_attackPendingDuration);
		StartCoroutine(AttackMove());
		//wait for next attack;
		yield return null;
	}
}
