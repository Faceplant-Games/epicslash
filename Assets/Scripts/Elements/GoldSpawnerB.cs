using UnityEngine;
using System.Collections;

public class GoldSpawnerB : MonoBehaviour {
	public GoldBag goldBagPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(int nbGoldBags) {
		for (int i = 0; i < nbGoldBags; i++) {
			Vector3 random1 = new Vector3 (Random.Range (0, 2), Random.Range (0, 2), Random.Range (0, 2));
			Vector3 pos = this.gameObject.transform.position + random1;
			Instantiate (goldBagPrefab, pos, this.gameObject.transform.rotation);
		}
	}
}
