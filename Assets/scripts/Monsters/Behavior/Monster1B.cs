using UnityEngine;
using System.Collections;

public class Monster1B : AbstractMonster 
{
	private GroundMovingB _groundMovingB;


	// Use this for initialization
	void Start () 
	{
		_groundMovingB = GetComponent<GroundMovingB>();
	}

	// Update is called once per frame
	void Update () 
	{

	}

	void Move(Vector3 position)
	{
		_groundMovingB.Move(position);
	}

	public override int  Experience(){
		return 0;
	}

	public override void Spawn(){

	}

	public override void Die(){

	}


}

