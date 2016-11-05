using UnityEngine;
using System.Collections;

public class GroundMovingB : MovingB 
{
	public NavMeshAgent MyNavMeshAgent;

	// Use this for initialization
	void Start () 
	{
		MyNavMeshAgent = GetComponent<NavMeshAgent>();
	}

	public override void Move (Vector3 position)
	{
		base.Move (position);
		MyNavMeshAgent.destination = position;
	}
}
