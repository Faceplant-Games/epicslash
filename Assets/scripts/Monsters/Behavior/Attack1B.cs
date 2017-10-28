﻿using UnityEngine;
using System.Collections;

/**
 * spiders 
 */
public class Attack1B : MonoBehaviour 
{
	private float _attackRate = 5f;
	private float _attackPendingDuration = 2f;
	private float _attackDistance = 3f;
	private float _attackTravelDuration = 2f;

	private IEnumerator _attackCoroutine;
	private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
	private GameObject player;
	private GroundMovingB _groundMovingB;

	private bool Attacking = false;
	// Use this for initialization
	void Start () 
	{
		_groundMovingB = GetComponent<GroundMovingB>();
		player = GameObject.FindGameObjectWithTag("MainCamera");
	}

	public void Attack(GameObject player)
	{
		Attacking = true;
		if (_attackCoroutine != null)
		{
			StopCoroutine(_attackCoroutine);
		}
		_attackCoroutine = AttackCoroutine(player);
		StartCoroutine(_attackCoroutine);
	}

	private void DisplayAttackSign()
	{
		//
	}

	void Update()
	{
		if (Vector3.Distance(player.transform.position, transform.position) > 3f && Attacking == false)
		{
			_groundMovingB.Move(player.transform.position);
		}
		else if (Attacking == false)
		{
			_groundMovingB.MyNavMeshAgent.Stop();
			Attack(player);
		}
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
		transform.LookAt(target.transform)
;		yield return null;
	}

	private IEnumerator AttackMove(GameObject player)
	{
		_groundMovingB.MyNavMeshAgent.Stop();
		float t = 0;
		Vector3 InitialPosition = transform.position;
		Vector3 FinalPosition = transform.position + transform.forward * Vector3.Distance(transform.position, player.transform.position) * 2;
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
		StartCoroutine(AttackMove(player));
		yield return new WaitForSeconds(5f);
		Attack(player);
		yield return null;
	}
}

