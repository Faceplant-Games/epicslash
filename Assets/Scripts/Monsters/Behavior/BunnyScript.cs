using System.Collections;
using UnityEngine;

/**
 * lapin
 * */

[RequireComponent(typeof(GroundMovingB), typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class BunnyScript : AbstractMonster 
{
	private GroundMovingB _groundMovingB;
	public Vector3 EscapePosition;
	private Animator _animator;

	public enum Monster2State
	{
		LookingForGold,
		Escaping
	}
	public Monster2State MyState;

	private float _timer;

	
	// Use this for initialization
	private void Start ()
	{
		_groundMovingB = GetComponent<GroundMovingB>();
		EscapePosition = transform.position;
		MyState = Monster2State.LookingForGold;
		_animator = GetComponentInChildren<Animator>();
        Hp = 2;
        Experience = 612;
        Malus = 15;
	}

	private GameObject NearestGoldBag()
	{
		var bags = FindObjectsOfType<GoldBag>();
		GameObject nearestBag = null;
		foreach (var i in bags)
		{
			if (nearestBag == null)
			{
				nearestBag = i.gameObject;
			}
			else if (Vector3.Distance(transform.position, i.gameObject.transform.position) < Vector3.Distance(nearestBag.transform.position, transform.position))
			{
				nearestBag = i.gameObject;
			}
		}
		return nearestBag;
	}


	// Update is called once per frame
	private void Update () 
	{
		
		_timer += Time.deltaTime;
		if (!(_timer > 1)) return;
		
		_timer = 0;
		if (MyState == Monster2State.LookingForGold)
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
			if (transform.position == EscapePosition && MyState == Monster2State.Escaping)
				MyState = Monster2State.LookingForGold;
		}

	}

    public override void OnTriggerEnter(Collider collision) 
	{
        base.OnTriggerEnter(collision);
		if (collision.gameObject.GetComponent<GoldBag>() != null && MyState == Monster2State.LookingForGold)
		{
            _animator.SetTrigger ("steal");
            StartCoroutine(Steal(collision));
		}
	}

	private IEnumerator Steal(Collider collision)
    {
        var asi = _animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(asi.length + asi.normalizedTime);
	    Game.GameManager.LoseExperience(Malus);
	    MyState = Monster2State.Escaping;
        if (collision != null)
        {
            Destroy(collision.gameObject);
        }
    }    
}
