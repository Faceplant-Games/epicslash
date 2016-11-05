using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GroundMovingB), typeof(Attack1B))]
public class Monster1B : AbstractMonster 
{
	private GroundMovingB _groundMovingB;
	private Attack1B _attack1B;


	// Use this for initialization
	void Start () 
	{
		_groundMovingB = GetComponent<GroundMovingB>();
		_attack1B = GetComponent<Attack1B>();
	}

	// Update is called once per frame
	void Update () 
	{

	}

	public void Move(Vector3 position)
	{
		_groundMovingB.Move(position);
	}

	public void Attack(GameObject target)
	{
		_attack1B.Attack(target);
	}

	public override int  Experience(){
		return 0;
	}

	public override void Spawn(){

	}

	public override void Die(){

	}


}

