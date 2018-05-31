using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GroundMovingB : MovingB 
{
    //[HideInInspector]
    public NavMeshAgent MyNavMeshAgent;


    private Animator _animator;

    // Use this for initialization
	private void Start () 
	{
		MyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

	public override void Move (Vector3 position)
    {
        base.Move (position);
        MyNavMeshAgent.isStopped = false;
        MyNavMeshAgent.destination = position;
	}
}
