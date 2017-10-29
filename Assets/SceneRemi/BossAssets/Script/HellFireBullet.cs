using UnityEngine;
using System.Collections;

public class HellFireBullet : MonoBehaviour 
{
	private float speed = 0;
	float timer = 0;
	float lifeTime = 10;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(qszdefrgtujiklop());
	}

	private IEnumerator qszdefrgtujiklop()
	{
		yield return new WaitForSeconds(2f);
		speed = 10;
	}

	void FixedUpdate () {
		float distanceThisFrame = speed * Time.fixedDeltaTime;
		//RaycastHit hit = new RaycastHit ();

		/*// At each frame, we cast a ray forward from where we are to where we will be next frame
		if (Physics.Raycast (transform.position, transform.forward, out hit, distanceThisFrame)) {
			if (hit.transform.gameObject.GetComponent<AbstractMonster>() != null) {
				print("Boom");
				AbstractMonster monster = hit.transform.gameObject.GetComponent<AbstractMonster>();
				monster.Die ();
				Destroy (gameObject);
			}
			else if (hit.transform.gameObject.GetComponent<ExplosiveSurface>() != null)
			{
				hit.transform.gameObject.GetComponent<ExplosiveSurface>().TriggerExplosion(hit.point);
				Destroy (gameObject);
			}
			else if (hit.transform.gameObject.GetComponent<ExplosiveWings>() != null)
			{
				hit.transform.gameObject.GetComponent<ExplosiveWings>().TriggerExplosion(hit.point);
				Destroy (gameObject);
			}
			else if (hit.transform.gameObject != null) {

				Destroy (gameObject);
			}*
		} else {*/
			transform.position += transform.forward * distanceThisFrame;
		//}

		timer += Time.fixedDeltaTime;
		if (timer > lifeTime) {
			Destroy(gameObject);
		}
	}
}
