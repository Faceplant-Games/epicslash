using UnityEngine;
using System.Collections;

public class FlyingMovingB : MovingB 
{
    public UnityEngine.AI.NavMeshAgent MyNavMeshAgent;

    void Start()
    {
        MyNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override void Move(Vector3 position)
	{
        base.Move(position);
        MyNavMeshAgent.isStopped = false;
        MyNavMeshAgent.destination = position;
    }
    
}
