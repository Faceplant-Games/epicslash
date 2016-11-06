using UnityEngine;
using System.Collections;

public class DONOTCOMMIT_TEst : MonoBehaviour 
{
	public Monster1B monster;
	public GameObject player;
	public GameObject targetPosition;


	// Use this for initialization
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			monster.Move(targetPosition.transform.position);
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			monster.Attack(player);
		}
	}
}
