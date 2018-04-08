using UnityEngine;
using System.Collections;

/**
 *spiders 
 */
[RequireComponent(typeof(GroundMovingB), typeof(Animator))]
public class Monster1B : AbstractMonster
{
    private GroundMovingB _groundMovingB;
    private GameObject player;
    private Animator animator;

    private float _attackRate = 5f;
    private float _attackPendingDuration = 2f;
    private float _attackDistance = 3f;
    private float _attackTravelDuration = 2f;

    private IEnumerator _attackCoroutine;
    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    // Use this for initialization
    void Start()
    {
        _groundMovingB = GetComponent<GroundMovingB>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("MainCamera");
        base.hp = 1;
        base.experience = 100;
        base.malus = 1;
    }


    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > 2f)
        {
            Move(player.transform.position);
        }
        else
        {
           AttackOrMove(player);
        }
    }

    public void Move(Vector3 position)
    {
        _groundMovingB.Move(position);
    }

    public void AttackOrMove(GameObject target)
    {
        StartCoroutine(AttackCoroutine());
    }
    
    IEnumerator AttackCoroutine()
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        if (!asi.IsName("Attack"))
        {
            _groundMovingB.MyNavMeshAgent.isStopped = true;
            animator.SetTrigger("attack");
            yield return new WaitForSeconds(asi.length + asi.normalizedTime);
            GameManager playerM = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            playerM.LoseExperience(base.malus);
            _groundMovingB.Move(player.transform.position);
        }
    }

    /*
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager playerM = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            playerM.LoseExperience(base.malus);

            
            animator.SetBool("shouldMove", false);
            GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            player.LevelDown(base.malus);
        }
    }*/
}

