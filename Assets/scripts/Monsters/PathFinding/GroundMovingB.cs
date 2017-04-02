using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class GroundMovingB : MovingB 
{
	[HideInInspector]
	public UnityEngine.AI.NavMeshAgent MyNavMeshAgent;
	 
	// Use this for initialization
	void Start () 
	{
		MyNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	public override void Move (Vector3 position)
	{
		base.Move (position);
		MyNavMeshAgent.Resume();
		MyNavMeshAgent.destination = position;
	}
}
