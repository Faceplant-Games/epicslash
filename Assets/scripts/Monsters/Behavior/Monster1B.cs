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
		player = GameObject.FindGameObjectWithTag("MainCamera");
	}

	public void Move(Vector3 position)
	{
		_groundMovingB.Move(position);
	}

	public void Attack(GameObject target)
	{
		_attack1B.Attack(target);
        GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        player.levelDown (1);
	}

	public override int  Experience(){
		return 0;
	}

	public override void Spawn(){

	}

	public override void Die(){

        GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        if ( player != null){
			player.levelUp (100);
		}
        DestroyImmediate(this.gameObject);
		//FIXME AJOUT sac de gold?
	}


}

