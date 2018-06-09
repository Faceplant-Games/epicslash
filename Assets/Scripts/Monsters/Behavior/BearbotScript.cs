using UnityEngine;


/**
 *Bearbot 
 */
[RequireComponent(typeof(FlyingMovingB), typeof(Attack3B))]
public class BearbotScript : AbstractMonster
{
    private GameObject _player;
    private const float AttackRange = 40f;
    private const float ShortRange = 10f;
    private FlyingMovingB _flyingMovingB;
    private Attack3B _attack3B;
    private const float RateOfFire = 2;
    private float _t;
    private float _direction = -1;
    private Vector3 _targetLocation;
    private bool _isThereAPlaceToReach = false;

    public override string Name
    {
        get { return "Bearbot"; }
    }

    private void Start()
    {
        _player = Camera.main.gameObject;
        _flyingMovingB = GetComponent<FlyingMovingB>();
        _attack3B = GetComponent<Attack3B>();
        Hp = 5;
        Experience = 2539;
        Malus = 0;
        _targetLocation = _player.transform.position;
    }

    private void Move(Vector3 position)
    {
        _flyingMovingB.Move(position);
    }

    private void Update()
    {
        _t += Time.deltaTime;

        if (Vector3.Distance(_targetLocation, transform.position) < 0.5f)
        {
            _flyingMovingB.MyNavMeshAgent.isStopped = true;
            _isThereAPlaceToReach = false;
        }

        if (_isThereAPlaceToReach)
        {
            if (_flyingMovingB.MyNavMeshAgent.isStopped)
            {
                Move(_targetLocation);
            }
        }
        else if (_flyingMovingB.MyNavMeshAgent.isStopped)
        {
            if (_t > RateOfFire)
            {
                _t = 0;
                Attack(_player);
                FindNextLocation();
            }
        }
        else if (Vector3.Distance(_player.transform.position, transform.position) > AttackRange)
        {
            Move(_targetLocation);
        }
        else
        {
            _flyingMovingB.MyNavMeshAgent.isStopped = true;
            _isThereAPlaceToReach = false;
        }
    }

    private void FindNextLocation()
    {
        _direction *= -1;
        _isThereAPlaceToReach = true;
        Vector3 delta = ComputeDelta();
        _targetLocation = transform.position + delta;
    }

    private Vector3 ComputeDelta()
    {
        Vector3 delta;
        if (Vector3.Distance(_player.transform.position, transform.position) > ShortRange)
        {
            delta = (_player.transform.position - transform.position) * 0.1f;
            delta.y = 0;
            if (_direction > 0)
            {
                delta.x = delta.x * 3;
            }
            else
            {
                delta.z = delta.z * 3;
            }
        }
        else
        {
            delta = (_player.transform.position - transform.position) * 2f;
            delta.z = _direction * delta.x;
            delta.y = 0;
            delta.x = _direction * delta.z;
            _targetLocation = transform.position + delta;
        }
        return delta;
    }

    private void Attack(GameObject target)
    {
        this.transform.LookAt(target.transform);
        _attack3B.Attack(target);
    }
}