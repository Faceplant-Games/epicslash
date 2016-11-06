using UnityEngine;
using System.Collections;

public class SpawnerB : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Spawn(string prefabName){
		Vector3 random1 = new Vector3 (Random.Range (-5, 5),0, Random.Range (-5, 5));
		Vector3 pos = this.gameObject.transform.position + random1;
		Instantiate (Resources.Load (prefabName), pos, this.gameObject.transform.rotation);
	}
}