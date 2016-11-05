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
		Monster[] monsters = GameObject.FindObjectsOfType(typeof(Monster)) as Monster[];
		foreach (Monster monster in monsters) {
			// Is touching ?
			float dist = Vector3.Distance(transform.position, monster.gameObject.transform.position);
			if (dist < 1) { // Touching = under 1 distance unit
				monster.die();
			}
		}
			


	}
}
