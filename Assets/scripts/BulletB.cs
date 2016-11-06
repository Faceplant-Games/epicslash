using UnityEngine;
using System.Collections;

public class BulletB : MonoBehaviour {

	float timer = 0;
	float lifeTime = 10;
	float speed = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float distanceThisFrame = speed * Time.fixedDeltaTime;
		RaycastHit hit = new RaycastHit ();

		// At each frame, we cast a ray forward from where we are to where we will be next frame
		if (Physics.Raycast (transform.position, transform.forward, out hit, distanceThisFrame)) {
			if (hit.transform.gameObject.GetComponent<AbstractMonster>() != null) {
                print("Boom");
				AbstractMonster monster = hit.transform.gameObject.GetComponent<AbstractMonster>();
				monster.Die ();
				Destroy (gameObject);
			}
			else if (hit.transform.gameObject != null) {

				Destroy (gameObject);
			}
		} else {
			transform.position += transform.forward * distanceThisFrame;
		}

		timer += Time.fixedDeltaTime;
		if (timer > lifeTime) {
			Destroy(gameObject);
		}
	}
}
