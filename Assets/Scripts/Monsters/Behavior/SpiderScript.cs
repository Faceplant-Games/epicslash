using UnityEngine;
using System.Collections;

/**
 *spiders 
 */
[RequireComponent(typeof(GroundMovingB), typeof(Animator))]
public class SpiderScript : AbstractMonster
{
    private GroundMovingB _groundMovingB;
    private GameObject player;
    private Animator animator;

    private IEnumerator _attackCoroutine;

    public override string Name { get { return "Spider"; } }

    // Use this for initialization
    void Start()
    {
        _groundMovingB = GetComponent<GroundMovingB>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("MainCamera");
        base.hp = 1;
        base.experience = 31;
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
}

