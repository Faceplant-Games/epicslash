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
		Vector3 random1 = new Vector3 (Random.Range (-25, 25),0, Random.Range (-25, 25));
		Vector3 pos = this.gameObject.transform.position + random1;
        if ( prefabName!=null &&prefabName != "")
		    Instantiate (Resources.Load (prefabName), pos, this.gameObject.transform.rotation);
	}
}