using UnityEngine;
using System.Collections;

public class WeaponB : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// TODO if touching a mob, kill it

		// Get all mobs
		GameObject[] monsters = GameObject.FindObjectsOfType<Monster>();
		foreach (GameObject monster in monsters) {
			// Is touching ?
			float dist = Vector3.Distance(transform.position, monster.transform.position);
			if (dist < 1) { // Touching = under 1 distance unit
				// TODO : monster.die();
			}
		}
			


	}
}
