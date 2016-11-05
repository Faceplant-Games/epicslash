using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GroundMovingB), typeof(Attack1B))]
public class Monster1B : AbstractMonster 
{
	private GroundMovingB _groundMovingB;
	private Attack1B _attack1B;
	private GameObject player;
	private int hp = 1;


	// Use this for initialization
	void Start () 
	{
		_groundMovingB = GetComponent<GroundMovingB>();
		_attack1B = GetComponent<Attack1B>();
		player = GameObject.FindGameObjectWithTag("Player");
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
		PlayerB player = GameObject.FindObjectOfType(typeof(PlayerB)) as PlayerB ;
		if ( player != null){
			player.levelUp (1);
		}

		//FIXME AJOUT sac de gold?
	}


}

