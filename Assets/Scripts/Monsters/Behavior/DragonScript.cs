using UnityEngine;


/**
 *  Dragon
 * */
[RequireComponent(typeof(FlyingMovingB), typeof(Attack3B))]
public class DragonScript : AbstractMonster
{
    private GameObject _player;
    public float AttackRange = 10;
    private FlyingMovingB _flyingMovingB;
    private Attack3B _attack3B;
    public float RateOfFire = 4;
    private float _t;

    public override string Name { get { return "Dragon"; } }

    private void Start()
    {
        _player = Camera.main.gameObject;
        _flyingMovingB = GetComponent<FlyingMovingB>();
        _attack3B = GetComponent<Attack3B>();
        Hp = 10;
        Experience = 11357;
        Malus = 0;
    }

    private void Move(Vector3 position)
    {
        _flyingMovingB.Move(position);
    }

    private void Update()
    {
        if (_flyingMovingB.MyNavMeshAgent.isStopped)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < AttackRange)
            {
                _t += Time.deltaTime;
                if (_t > RateOfFire)
                {
                    _t = 0;
                    //_flyingMovingB.FaceObject(player.transform, 0.5f);
                    Attack(_player);
                }

            }
            else
            {
                Move(_player.transform.position - (_player.transform.position - transform.position) * 0.2f);
            }
        }
        else
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < AttackRange)
            {
                _flyingMovingB.MyNavMeshAgent.isStopped = true;
            }
            else
            {
                Move(_player.transform.position - (_player.transform.position - transform.position) * 0.2f);
            }
        }
    }

    private void Attack(GameObject target)
    {
        _attack3B.Attack(target);
    }

}
