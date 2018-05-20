using UnityEngine;
using System.Collections;

public class BulletB : MonoBehaviour {

	float timer = 0;
	float lifeTime = 10;

    public AudioSource audioSource;
    public AudioClip[] impact;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WeaponB>() != null || other.GetComponent<Camera>() != null)
        {
            return;
        }
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.fixedDeltaTime;
		if (timer > lifeTime) {
			Destroy(gameObject);
		}
	}
}
