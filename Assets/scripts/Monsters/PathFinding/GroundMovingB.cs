using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class GroundMovingB : MovingB 
{
	[HideInInspector]
	public NavMeshAgent MyNavMeshAgent;
	 
	// Use this for initialization
	void Start () 
	{
		MyNavMeshAgent = GetComponent<NavMeshAgent>();
	}

	public override void Move (Vector3 position)
	{
		base.Move (position);
		MyNavMeshAgent.Resume();
		MyNavMeshAgent.destination = position;
	}
}
