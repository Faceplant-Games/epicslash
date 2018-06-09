using UnityEngine;
using System.Collections;

/**
 *spiders 
 */
[RequireComponent(typeof(GroundMovingB), typeof(Animator))]
public class SpiderScript : AbstractMonster
{
    private GroundMovingB _groundMovingB;
    private GameObject _player;
    private Animator _animator;

    private IEnumerator _attackCoroutine;

    public override string Name { get { return "Spider"; } }

    // Use this for initialization
    private void Start()
    {
        _groundMovingB = GetComponent<GroundMovingB>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("MainCamera");
        Hp = 1;
        Experience = 31;
        Malus = 1;
    }


    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > 2f)
        {
            Move(_player.transform.position);
        }
        else
        {
           AttackOrMove();
        }
    }

    private void Move(Vector3 position)
    {
        _groundMovingB.Move(position);
    }

    private void AttackOrMove()
    {
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        var asi = _animator.GetCurrentAnimatorStateInfo(0);
        if (!asi.IsName("Attack"))
        {
            _groundMovingB.MyNavMeshAgent.isStopped = true;
            _animator.SetTrigger("attack");
            yield return new WaitForSeconds(asi.length + asi.normalizedTime);
            Game.GameManager.LoseExperience(Malus);
            _groundMovingB.Move(_player.transform.position);
        }
    }
}

