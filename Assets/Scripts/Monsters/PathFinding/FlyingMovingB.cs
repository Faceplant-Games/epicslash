using UnityEngine;
using System.Collections;

public class FlyingMovingB : MovingB 
{
    //[HideInInspector]
    public UnityEngine.AI.NavMeshAgent MyNavMeshAgent;
    //private IEnumerator Coroutine;
    //public float speed = 0.01f;
    //private WaitForEndOfFrame waitForEnedOfFrame = new WaitForEndOfFrame();
    //[HideInInspector]
    //public bool mooving;

    void Start()
    {
        MyNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    //   public IEnumerator FaceObject(Transform target, float duration)
    //{
    //	Vector3 direction = target.position - transform.position;
    //	Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
    //	float timer = 0;
    //	Quaternion initialRotation = transform.rotation;
    //	while(timer < duration)
    //	{
    //		yield return waitForEnedOfFrame;
    //		timer += Time.deltaTime;
    //		transform.rotation = Quaternion.Lerp(initialRotation, toRotation, timer/duration);
    //	}
    //	transform.rotation = toRotation;
    //	transform.LookAt(target);
    //	yield return null;
    //}


    public override void Move(Vector3 position)
	{
        //if (Coroutine != null)
        //{
        //	StopCoroutine(Coroutine);
        //}
        //Coroutine = MoveToCoroutine(position);
        //StartCoroutine(Coroutine);

        base.Move(position);
        MyNavMeshAgent.isStopped = false;
        MyNavMeshAgent.destination = position;
    }

	//private IEnumerator MoveToCoroutine(Vector3 position)
	//{
        //mooving = true;
        //StartCoroutine(FaceObject(Camera.main.transform, 0.5f));
        //yield return new WaitForSeconds(1);
        //float distance2 = Vector3.Distance(position, transform.position);
        //Vector3 initialPosition = transform.position;
        //float distance = 0;
        //while (distance < distance2)
        //{
        //    yield return waitForEnedOfFrame;
        //    distance += speed * Time.deltaTime;
        //    transform.position = initialPosition + (position - initialPosition) * (distance / distance2);
        //}
        //transform.position = position;
        //mooving = false;
        //yield return null;
    //}
}
