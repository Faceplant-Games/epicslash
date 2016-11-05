using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GroundMovingB), typeof(Collider), typeof(Rigidbody))]
public class Monster2B : AbstractMonster 
{
	private GroundMovingB _groundMovingB;
	public Vector3 EscapePosition;
	private int hp = 1;
	/*private GameObject _goldTarget;
	private GameObject goldTarget
	{
		get
		{
			if (_goldTarget == null)
			{
				_goldTarget = NearestGoldBag();
			}
			return _goldTarget;
		}
		set
		{
			_goldTarget = value;
		}
	}*/

	public enum Monster2State
	{
		LookingForGold,
		Escaping
	}
	public Monster2State myState;

	// Use this for initialization
	void Start () 
	{
		_groundMovingB = GetComponent<GroundMovingB>();
		EscapePosition = transform.position;
		myState = Monster2State.LookingForGold;
	}

	private GameObject NearestGoldBag()
	{
		GoldBag[] Bags = GameObject.FindObjectsOfType<GoldBag>();
		GameObject NearestBag = null;
		foreach (GoldBag i in Bags)
		{
			if (NearestBag == null)
			{
				NearestBag = i.gameObject;
			}
			else if (Vector3.Distance(transform.position, i.gameObject.transform.position) < Vector3.Distance(NearestBag.transform.position, transform.position))
			{
				NearestBag = i.gameObject;
			}
		}
		return NearestBag;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (myState == Monster2State.LookingForGold)
		{
			_groundMovingB.Move(NearestGoldBag().transform.position);
		}
		else
		{
			_groundMovingB.Move(EscapePosition);
		}
	}

	void OnTriggerEnter(Collider collision)
	{
		Debug.Log("Hello");
		if (collision.gameObject.GetComponent<GoldBag>() != null && myState == Monster2State.LookingForGold)
		{
			Debug.Log("Escape!");
			StealGold();
			Destroy(collision.gameObject);
		}
	}

	public void StealGold()
	{
		myState = Monster2State.Escaping;
		//StealGold
	}

	public override int  Experience()
	{
		return 0;
	}

	public override void Spawn()
	{

	}

	public override void Die()
	{
		PlayerB player = GameObject.FindObjectOfType(typeof(PlayerB)) as PlayerB ;
		if ( player != null){
			player.levelUp (1);
		}
		Destroy (this);
		//FIXME AJOUT sac de gold?
	}
}
