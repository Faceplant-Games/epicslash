using UnityEngine;
using System.Collections;

/**
 *spiders 
 */
[RequireComponent(typeof(GroundMovingB), typeof(Attack1B))]
public class Monster1B : AbstractMonster 
{
	private GroundMovingB _groundMovingB;
	private Attack1B _attack1B;
	private GameObject player;



    // Use this for initialization
    void Start () 
	{
		_groundMovingB = GetComponent<GroundMovingB>();
		_attack1B = GetComponent<Attack1B>();
		player = GameObject.FindGameObjectWithTag("MainCamera");
        base.hp = 1;
        base.experience = 100;
        base.malus = 1;
	}

	public void Move(Vector3 position)
	{
		_groundMovingB.Move(position);
	}

	public void Attack(GameObject target)
	{
		_attack1B.Attack(target);
        GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        player.LoseExperience (base.malus);
	}
}

