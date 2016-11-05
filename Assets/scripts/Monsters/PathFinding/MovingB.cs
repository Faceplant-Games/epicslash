using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class MovingB : MonoBehaviour 
{
	public virtual void Move(Vector3 position)
	{
		//Debug.Log("Moving to position : " + position);
	}
}
