using UnityEngine;
using UnityEngine.AI;

public class FlyingMovingB : MovingB 
{
    public NavMeshAgent MyNavMeshAgent;

    private void Start()
    {
        MyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void Move(Vector3 position)
	{
        base.Move(position);
        MyNavMeshAgent.isStopped = false;
        MyNavMeshAgent.destination = position;
    }
    
}
