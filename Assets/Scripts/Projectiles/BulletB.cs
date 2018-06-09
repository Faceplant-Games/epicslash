using UnityEngine;
using System.Collections;

public class BulletB : MonoBehaviour {
	private float _timer;
	private const float LifeTime = 10;

	public AudioSource AudioSource;
    public AudioClip[] Impact;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WeaponB>() != null 
            || other.GetComponent<Camera>() != null
            || other.tag.Equals("Ground"))
        {
            return;
        }
        AudioSource.PlayOneShot(Impact[Random.Range(0, Impact.Length) % Impact.Length], .5f);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	private void FixedUpdate () {
		_timer += Time.fixedDeltaTime;
		if (_timer > LifeTime) {
			Destroy(gameObject);
		}
	}
}
