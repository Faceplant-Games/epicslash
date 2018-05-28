using UnityEngine;
using System.Collections;
/**
 * lapin
 * */

[RequireComponent(typeof(GroundMovingB), typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class BunnyScript : AbstractMonster 
{
	private GroundMovingB _groundMovingB;
	public Vector3 EscapePosition;
	Animator animator;

    public override string Name { get { return "Bunny"; } }

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
		animator = this.GetComponentInChildren<Animator>();
        base.hp = 2;
        base.experience = 612;
        base.malus = 15;
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

    public override void OnTriggerEnter(Collider collision) 
	{
        base.OnTriggerEnter(collision);
		if (collision.gameObject.GetComponent<GoldBag>() != null && myState == Monster2State.LookingForGold)
		{
            animator.SetTrigger ("steal");
            StartCoroutine(Steal(collision));
		}
	}

    IEnumerator Steal(Collider collision)
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(asi.length + asi.normalizedTime);
        StealGold();
        GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        player.LoseExperience(base.malus);
        if (collision != null)
        {
            Destroy(collision.gameObject);
        }
    }

    public void StealGold()
    {
        myState = Monster2State.Escaping;
        //StealGold
        
    }
    
}
