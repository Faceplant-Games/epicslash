using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GroundMovingB), typeof(Collider), typeof(Rigidbody))]
public class Monster2B : AbstractMonster 
{
	private GroundMovingB _groundMovingB;
	public Vector3 EscapePosition;
	private int hp = 1;
	Animator anim;

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
		anim = this.GetComponentInChildren<Animator>();
		anim.SetTrigger ("doitsauter");

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


	float timer = 0;

	// Update is called once per frame
	void Update () 
	{
		
		timer += Time.deltaTime;
		if (timer > 1)
		{
			timer = 0;
			if (myState == Monster2State.LookingForGold)
			{
				if (NearestGoldBag() == null)
				{
					return;
				}
				_groundMovingB.Move(NearestGoldBag().transform.position);
			}
			else
			{
				_groundMovingB.Move(EscapePosition);
                if (transform.position == EscapePosition && myState == Monster2State.Escaping)
                    myState = Monster2State.LookingForGold;
			}
		}

	}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.GetComponent<GoldBag>() != null && myState == Monster2State.LookingForGold)
		{

			anim.SetTrigger ("vavoler");
			StealGold();
            GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            player.levelDown (100);
			Destroy(collision.gameObject);
		}
	}

	public void StealGold()
	{
		myState = Monster2State.Escaping;
		//StealGold

		anim.SetTrigger ("doitsauter");
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

        GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        if ( player != null){
			player.levelUp (1);
		}
		DestroyImmediate (this.gameObject);
		//FIXME AJOUT sac de gold?
	}
}
